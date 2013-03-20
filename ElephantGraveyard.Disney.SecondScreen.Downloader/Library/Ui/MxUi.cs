using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable FieldCanBeMadeReadOnly.Local
namespace ElephantGraveyard.Disney.SecondScreen.Downloader.Library.Ui
{
    public abstract class MXUIBase
    {
        public MXUILayerInfo layerInfo = new MXUILayerInfo();
    }

    [DebuggerDisplay (@"name = ""{name}"", frame = \{{frame.X}, {frame.Y}, {frame.Width}, {frame.Height}\}")]
    public class MXUILayerInfo
    {
        public String name;
        public Rect frame;
    }

    [DebuggerDisplay (@"contents = ""{contents.file}""")]
    public class MXUILayer : MXUIBase
    {
        public MXUIImage contents;
    }

    [DebuggerDisplay (@"file = ""{file}""")]
    public class MXUIImage
    {
        public MXUILayerInfo layerInfo = new MXUILayerInfo();
        public String file;
    }

    [DebuggerDisplay (@"subviews = \{Count = {subviews.Count}\}, layers = \{Count = {layers.Count}\}, buttons = \{Count = {buttons.Count}\}, sliders = \{Count = {sliders.Count}\}")]
    public class MXUIView : MXUIBase
    {
        public List<MXUIView> subviews = new List<MXUIView>();
        public List<MXUILayer> layers = new List<MXUILayer>();
        public List<MXUIButton> buttons = new List<MXUIButton>();
        public List<MXUISlider> sliders = new List<MXUISlider>();

        public void insertSubview (MXUIView view)
        {
            subviews.Add(view);
        }

        public void insertLayer (MXUILayer layer)
        {
            layers.Add(layer);
        }

        public void insertButton (MXUIButton button)
        {
            buttons.Add(button);
        }

        public void insertSlider (MXUISlider slider)
        {
            sliders.Add(slider);
        }

        public void insertImage (MXUIImage image)
        {
            var imageLayer = new MXUILayer();
            imageLayer.contents = image;
            imageLayer.layerInfo.frame = image.layerInfo.frame;
            imageLayer.layerInfo.name = image.layerInfo.name;
            layers.Add(imageLayer);
        }
    }

    [DebuggerDisplay (@"imageStates = \{Count = {imageStates.Count}\}")]
    public class MXUIButton : MXUIBase
    {
        [Flags]
        public enum IMAGE_STATE
        {
            NORMAL = 0,
            HIGHLIGHTED = 1,
            DISABLED = 2,
            SELECTED = 4
        };

        public Boolean maskedHitTest;
        public String selectorString;
        public Dictionary<IMAGE_STATE, MXUIImage> imageStates = new Dictionary<IMAGE_STATE, MXUIImage>();
    }

    [DebuggerDisplay (@"thumbStates = \{Count = {thumbStates.Count}\}, trackStates = \{Count = {trackStates.Count}\}")]
    public class MXUISlider : MXUIBase
    {
        public Rect trackRect;
        public Dictionary<int, MXUIImage> thumbStates = new Dictionary<int, MXUIImage>();
        public Dictionary<int, MXUIImage> trackStates = new Dictionary<int, MXUIImage>();
    }

    public class MXUIGallery : MXUIView
    {}

    [DebuggerDisplay (@"sequenceCount = {sequenceCount}, imageCount = {imageCount}, imageSequences = \{Count = {imageSequences.Count}\}")]
    public class MXUIFlipbook : MXUIView
    {
        public int sequenceCount;
        public int imageCount;
        public Rect imageBounds;
    }

    [DebuggerDisplay (@"zoomContent = {zoomContent}")]
    public class MXUIScrollView : MXUIView
    {
        public Boolean zoomContent;
        public Rect contentFrame;
    }

    [DebuggerDisplay (@"filename = ""{filename}""")]
    public class MXUIVideo : MXUIView
    {
        public String filename;
    }
}