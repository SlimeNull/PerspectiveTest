﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenCvSharp;
using PerspectiveTest.Geometry;
using PerspectiveTest.Helpers;
using PerspectiveTest.Models;
using WpfPoint = System.Windows.Point;

namespace PerspectiveTest.ViewModels
{
    [ObservableObject]
    public partial class MainWindowModel
    {
        private readonly Random _random = new();

        private readonly DependentValue<Quad, Quad, Mat> _sourceToDestinationMatrix = new DependentValue<Quad, Quad, Mat>((sourceQuad, destQuad) =>
        {
            return Cv2.GetPerspectiveTransform(
                ((System.Windows.Point[])[sourceQuad.LeftTop, sourceQuad.LeftBottom, sourceQuad.RightTop, sourceQuad.RightBottom]).Select(p => new Point2f((float)p.X, (float)p.Y)),
                ((System.Windows.Point[])[sourceQuad.LeftTop, sourceQuad.LeftBottom, sourceQuad.RightTop, sourceQuad.RightBottom]).Select(p => new Point2f((float)p.X, (float)p.Y)));
                //((System.Windows.Point[])[destQuad.LeftTop, destQuad.RightTop, destQuad.RightBottom, destQuad.LeftBottom]).Select(p => new Point2f((float)p.X, (float)p.Y)));
        });

        private readonly DependentValue<Quad, Mat> _normalizedToDestinationMatrix = new DependentValue<Quad, Mat>((destQuad) =>
        {
            return Cv2.GetPerspectiveTransform(
                [new Point2f(0, 0), new Point2f(1, 0), new Point2f(1, 1), new Point2f(0, 1)],
                ((System.Windows.Point[])[destQuad.LeftTop, destQuad.RightTop, destQuad.RightBottom, destQuad.LeftBottom]).Select(p => new Point2f((float)p.X, (float)p.Y)));
        });

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NormalizedToSourceMatrix))]
        [NotifyPropertyChangedFor(nameof(SourceToDestinationMatrix))]
        private double _sourceAreaX1 = 10;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NormalizedToSourceMatrix))]
        [NotifyPropertyChangedFor(nameof(SourceToDestinationMatrix))]
        private double _sourceAreaX2 = 100;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NormalizedToSourceMatrix))]
        [NotifyPropertyChangedFor(nameof(SourceToDestinationMatrix))]
        private double _sourceAreaX3 = 100;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NormalizedToSourceMatrix))]
        [NotifyPropertyChangedFor(nameof(SourceToDestinationMatrix))]
        private double _sourceAreaX4 = 10;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NormalizedToSourceMatrix))]
        [NotifyPropertyChangedFor(nameof(SourceToDestinationMatrix))]
        private double _sourceAreaY1 = 10;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NormalizedToSourceMatrix))]
        [NotifyPropertyChangedFor(nameof(SourceToDestinationMatrix))]
        private double _sourceAreaY2 = 10;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NormalizedToSourceMatrix))]
        [NotifyPropertyChangedFor(nameof(SourceToDestinationMatrix))]
        private double _sourceAreaY3 = 100;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NormalizedToSourceMatrix))]
        [NotifyPropertyChangedFor(nameof(SourceToDestinationMatrix))]
        private double _sourceAreaY4 = 100;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SourceToDestinationMatrix))]
        private double _destinationAreaX1 = 10;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SourceToDestinationMatrix))]
        private double _destinationAreaX2 = 100;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SourceToDestinationMatrix))]
        private double _destinationAreaX3 = 100;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SourceToDestinationMatrix))]
        private double _destinationAreaX4 = 10;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SourceToDestinationMatrix))]
        private double _destinationAreaY1 = 10;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SourceToDestinationMatrix))]
        private double _destinationAreaY2 = 10;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SourceToDestinationMatrix))]
        private double _destinationAreaY3 = 100;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SourceToDestinationMatrix))]
        private double _destinationAreaY4 = 100;

        public Mat NormalizedToSourceMatrix => _normalizedToDestinationMatrix.Get(
            new Quad(new WpfPoint(SourceAreaX1, SourceAreaY1), new WpfPoint(SourceAreaX2, SourceAreaY2), new WpfPoint(SourceAreaX3, SourceAreaY3), new WpfPoint(SourceAreaX4, SourceAreaY4)));

        public Mat SourceToDestinationMatrix => _sourceToDestinationMatrix.Get(
            new Quad(new WpfPoint(SourceAreaX1, SourceAreaY1), new WpfPoint(SourceAreaX2, SourceAreaY2), new WpfPoint(SourceAreaX3, SourceAreaY3), new WpfPoint(SourceAreaX4, SourceAreaY4)),
            new Quad(new WpfPoint(DestinationAreaX1, DestinationAreaY1), new WpfPoint(DestinationAreaX2, DestinationAreaY2), new WpfPoint(DestinationAreaX3, DestinationAreaY3), new WpfPoint(DestinationAreaX4, DestinationAreaY4)));

        public ObservableCollection<RefPoint> SourceAreaPoints { get; } = new();

        public MappedCollection<RefPoint, ConvertedValue<RefPoint, WpfPoint>> DestinationAreaPoints { get; }

        public MainWindowModel()
        {
            DestinationAreaPoints
                = new MappedCollection<RefPoint, ConvertedValue<RefPoint, WpfPoint>>(SourceAreaPoints, sp => new(sp, sp => TransformPointFromSourceToDestinationArea(SourceToDestinationMatrix, new WpfPoint(sp.X, sp.Y))));
        }

        private WpfPoint TransformPointFromSourceToDestinationArea(Mat transformMatrix, WpfPoint point)
        {
            Mat matrixPoint = new Mat<double>(3, 1, new Scalar(point.X, point.Y, 1));
            Mat transformedPoint = transformMatrix * matrixPoint;

            var result = new WpfPoint(
                transformedPoint.At<double>(0, 0),
                transformedPoint.At<double>(1, 0));

            return result;
        }

        [RelayCommand]
        private void AddPoint()
        {
            var randomNormalizedPoint = new WpfPoint(_random.NextDouble(), _random.NextDouble());
            var randomSourcePoint = TransformPointFromSourceToDestinationArea(NormalizedToSourceMatrix, randomNormalizedPoint);

            SourceAreaPoints.Add(new RefPoint((int)randomSourcePoint.X, (int)randomSourcePoint.Y));
        }

        [RelayCommand]
        private void RemovePoint()
        {
            SourceAreaPoints.RemoveAt(SourceAreaPoints.Count - 1);
        }
    }
}