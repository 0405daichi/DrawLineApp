using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace WpfApp3
{
    public class DesignerCanvas : Canvas
    {
        public FrameworkElement CurrentElement { get; set; }
        public Point BeforeMovingPosition { get; set; }
        public ObservableCollection<MyLine> List { get; }
        public ObservableCollection<MyLine> SelectedList { get; }
        public DesignerCanvas()
        {
            this.AllowDrop = true;
            this.MouseLeftButtonDown += LDown;
            this.MouseMove += Canvas_MouseMove;
            this.MouseLeftButtonUp += LeftButtonUp;
            List = new ObservableCollection<MyLine>();
            SelectedList = new ObservableCollection<MyLine>();
        }

        private void LDown(object sender, MouseEventArgs e)
        {
            BeforeMovingPosition = e.GetPosition(this);
            var element = e.Source as FrameworkElement;
            CurrentElement = element;
            e.Handled = true;
            if (CurrentElement != null && CurrentElement.ToString() == "System.Windows.Shapes.Path")
            {
                var shape = CurrentElement.DataContext as MyLine;
                if (!List.Contains(shape))
                    List.Add(shape);

                shape.IsSelected = true;
                if (!SelectedList.Contains(shape))
                    SelectedList.Add(shape);
            }
            if (CurrentElement != null && CurrentElement.ToString() == "WpfApp3.DesignerCanvas" && SelectedList.Any())
            {
                foreach (var i in SelectedList)
                {
                    i.IsSelected = false;
                }
                SelectedList.Clear();
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && CurrentElement != null && CurrentElement.ToString() == "System.Windows.Shapes.Path")
            {
                var currentPoint = e.GetPosition(this);
                var shape = CurrentElement.DataContext as MyLine;
                var newX = currentPoint.X;
                var newY = currentPoint.Y;

                if (newX < 0) newX = 0;
                if (newX > this.ActualWidth - CurrentElement.Width) newX = this.ActualWidth - CurrentElement.Width;

                if (newY < 0) newY = 0;
                if (newY > this.ActualHeight - CurrentElement.Height) newY = this.ActualHeight - CurrentElement.Height;

                if (CurrentElement.Name == "StartPoint")
                {
                    shape.Start.Point = new Point(newX, newY);
                    SnapConnection();
                }
                else if (CurrentElement.Name == "EndPoint")
                {
                    shape.End.Point = new Point(newX, newY);
                    SnapConnection();
                }
                else if (CurrentElement.Name == "Line")
                {
                    ShiftGeometry(newX, newY, shape);
                    BeforeMovingPosition = currentPoint;
                    SnapConnection();
                }
            }
        }

        private void LeftButtonUp(object sender, MouseEventArgs e)
        {
            CurrentElement = null;
        }

        private void ShiftGeometry(double newX, double newY, MyLine shape)
        {
            var diffX = newX - BeforeMovingPosition.X;
            var diffY = newY - BeforeMovingPosition.Y;
            var v = new Vector(diffX, diffY);
            foreach (var selectedLine in SelectedList)
            {
                selectedLine.Start.Point += v;
                selectedLine.End.Point += v;
            }
        }

        private void SnapConnection()
        {
            if (SelectedList.Any() && SelectedList.Count == 1)
            {
                var movingShape = SelectedList[0];
                var AccessLineList = List.Where(v => v != movingShape);
                if (CurrentElement.Name == "StartPoint" || CurrentElement.Name == "Line")
                {
                    foreach (var AccessLine in AccessLineList)
                    {
                        var StoS = AccessLine.Start.Point - movingShape.Start.Point;
                        var StoE = AccessLine.End.Point - movingShape.Start.Point;
                        if (StoS.Length < 10)
                        {
                            movingShape.Start.Point = AccessLine.Start.Point;
                            if (CurrentElement.Name == "Line")
                                movingShape.End.Point += StoS;

                            if (!movingShape.Start.AccessLine.Contains(AccessLine))
                                movingShape.Start.AccessLine.Add(AccessLine);
                            if (!AccessLine.Start.AccessLine.Contains(movingShape))
                                AccessLine.Start.AccessLine.Add(movingShape);
                        }

                        if (StoE.Length < 10)
                        {
                            movingShape.Start.Point = AccessLine.End.Point;
                            if (CurrentElement.Name == "Line")
                                movingShape.End.Point += StoE;

                            if (!movingShape.Start.AccessLine.Contains(AccessLine))
                                movingShape.Start.AccessLine.Add(AccessLine);
                            if (!AccessLine.End.AccessLine.Contains(movingShape))
                                AccessLine.End.AccessLine.Add(movingShape);
                        }
                    }
                }
                if (CurrentElement.Name == "EndPoint" || CurrentElement.Name == "Line")
                {
                    foreach (var AccessLine in AccessLineList)
                    {
                        var EtoS = AccessLine.Start.Point - movingShape.End.Point;
                        var EtoE = AccessLine.End.Point - movingShape.End.Point;
                        if (EtoS.Length < 10)
                        {
                            movingShape.End.Point = AccessLine.Start.Point;
                            if (CurrentElement.Name == "Line")
                                movingShape.Start.Point += EtoS;

                            if (!movingShape.End.AccessLine.Contains(AccessLine))
                                movingShape.End.AccessLine.Add(AccessLine);
                            if (!AccessLine.Start.AccessLine.Contains(movingShape))
                                AccessLine.Start.AccessLine.Add(movingShape);
                        }

                        if (EtoE.Length < 10)
                        {
                            movingShape.End.Point = AccessLine.End.Point;
                            if (CurrentElement.Name == "Line")
                                movingShape.Start.Point += EtoE;

                            if (!movingShape.End.AccessLine.Contains(AccessLine))
                                movingShape.End.AccessLine.Add(AccessLine);
                            if (!AccessLine.End.AccessLine.Contains(movingShape))
                                AccessLine.End.AccessLine.Add(movingShape);
                        }
                    }
                }

                CheckConnectionStatus(AccessLineList, movingShape);
            }
            else if (SelectedList.Any() && SelectedList.Count > 1)
            {
                foreach (var movingShape in SelectedList)
                {
                    var AccessLineList = List.Where(v => v != movingShape).Where(v => v.IsSelected == false);
                    var ConnectedList = SelectedList.Where(v => v != movingShape);
                    if (CurrentElement.Name == "StartPoint" || CurrentElement.Name == "Line")
                    {
                        foreach (var AccessLine in AccessLineList)
                        {
                            var StoS = AccessLine.Start.Point - movingShape.Start.Point;
                            var StoE = AccessLine.End.Point - movingShape.Start.Point;
                            if (StoS.Length < 10)
                            {
                                movingShape.Start.Point = AccessLine.Start.Point;
                                if (CurrentElement.Name == "Line")
                                {
                                    movingShape.End.Point += StoS;
                                }
                                if (CurrentElement.Name != "StartPoint")
                                {
                                    foreach (var connectedLine in ConnectedList)
                                    {
                                        connectedLine.Start.Point += StoS;
                                        connectedLine.End.Point += StoS;
                                    }
                                }
                                if (!movingShape.Start.AccessLine.Contains(AccessLine))
                                    movingShape.Start.AccessLine.Add(AccessLine);
                                if (!AccessLine.Start.AccessLine.Contains(movingShape))
                                    AccessLine.Start.AccessLine.Add(movingShape);
                            }
                            if (StoE.Length < 10)
                            {
                                movingShape.Start.Point = AccessLine.End.Point;
                                if (CurrentElement.Name == "Line")
                                {
                                    movingShape.End.Point += StoE;
                                }
                                if (CurrentElement.Name != "StartPoint")
                                {
                                    foreach (var connectedLine in ConnectedList)
                                    {
                                        connectedLine.Start.Point += StoE;
                                        connectedLine.End.Point += StoE;
                                    }
                                }
                                if (!movingShape.Start.AccessLine.Contains(AccessLine))
                                    movingShape.Start.AccessLine.Add(AccessLine);
                                if (!AccessLine.End.AccessLine.Contains(movingShape))
                                    AccessLine.End.AccessLine.Add(movingShape);
                            }
                        }
                    }
                    if (CurrentElement.Name == "EndPoint" || CurrentElement.Name == "Line")
                    {
                        foreach (var AccessLine in AccessLineList)
                        {
                            var EtoS = AccessLine.Start.Point - movingShape.End.Point;
                            var EtoE = AccessLine.End.Point - movingShape.End.Point;
                            if (EtoS.Length < 10)
                            {
                                movingShape.End.Point = AccessLine.Start.Point;
                                if (CurrentElement.Name == "Line")
                                {
                                    movingShape.Start.Point += EtoS;
                                }
                                if (CurrentElement.Name != "EndPoint")
                                {
                                    foreach (var connectedLine in ConnectedList)
                                    {
                                        connectedLine.Start.Point += EtoS;
                                        connectedLine.End.Point += EtoS;
                                    }
                                }
                                if (!movingShape.End.AccessLine.Contains(AccessLine))
                                    movingShape.End.AccessLine.Add(AccessLine);
                                if (!AccessLine.Start.AccessLine.Contains(movingShape))
                                    AccessLine.Start.AccessLine.Add(movingShape);
                            }
                            if (EtoE.Length < 10)
                            {
                                movingShape.End.Point = AccessLine.End.Point;
                                if (CurrentElement.Name == "Line")
                                {
                                    movingShape.Start.Point += EtoE;
                                }
                                if (CurrentElement.Name != "EndPoint")
                                {
                                    foreach (var connectedLine in ConnectedList)
                                    {
                                        connectedLine.Start.Point += EtoE;
                                        connectedLine.End.Point += EtoE;
                                    }
                                }
                                if (!movingShape.End.AccessLine.Contains(AccessLine))
                                    movingShape.End.AccessLine.Add(AccessLine);
                                if (!AccessLine.End.AccessLine.Contains(movingShape))
                                    AccessLine.End.AccessLine.Add(movingShape);
                            }
                        }
                    }

                    CheckConnectionStatus(AccessLineList, movingShape);
                }
            }
        }

        private void CheckConnectionStatus(IEnumerable<MyLine> AccsessLineList, MyLine movingShape)
        {
            foreach (var AccessLine in AccsessLineList)
            {
                if (movingShape.Start.AccessLine.Contains(AccessLine) || movingShape.Start.AccessLine.Contains(AccessLine))
                {
                    if (AccessLine.Start.AccessLine.Contains(movingShape))
                    {
                        if (movingShape.Start.Point != AccessLine.Start.Point)
                        {
                            movingShape.Start.AccessLine.Remove(AccessLine);
                            AccessLine.Start.AccessLine.Remove(movingShape);
                        }
                    }
                    if (AccessLine.End.AccessLine.Contains(movingShape))
                    {
                        if (movingShape.Start.Point != AccessLine.End.Point)
                        {
                            movingShape.Start.AccessLine.Remove(AccessLine);
                            AccessLine.End.AccessLine.Remove(movingShape);
                        }
                    }
                }
                
                if (movingShape.End.AccessLine.Contains(AccessLine) || movingShape.End.AccessLine.Contains(AccessLine))
                {
                    if (AccessLine.Start.AccessLine.Contains(movingShape))
                    {
                        if (movingShape.End.Point != AccessLine.Start.Point)
                        {
                            movingShape.End.AccessLine.Remove(AccessLine);
                            AccessLine.Start.AccessLine.Remove(movingShape);
                        }
                    }
                    if (AccessLine.End.AccessLine.Contains(movingShape))
                    {
                        if (movingShape.End.Point != AccessLine.End.Point)
                        {
                            movingShape.End.AccessLine.Remove(AccessLine);
                            AccessLine.End.AccessLine.Remove(movingShape);
                        }
                    }
                }
            }
        }
    }
}
