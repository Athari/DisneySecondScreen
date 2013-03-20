using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using ElephantGraveyard.Disney.SecondScreen.Downloader.Library.Ui;

// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
namespace ElephantGraveyard.Disney.SecondScreen.Downloader.Library.Parser
{
    public class MxCsiParser : Object
    {
        public static List<string> LayerTypeNames = new List<string> { "LayerType_Layer", "LayerType_Group", "LayerType_ButtonGroup", "LayerType_SegmentGroup", "LayerType_CommonGroup", "LayerType_Link", "LayerType_ButtonImage", "LayerType_ButtonOverlay", "LayerType_View", "LayerType_Tiled", "LayerType_ZoomingSegmentGroup", "LayerType_ScrollingView", "LayerType_VideoLayer", "LayerType_GalleryGroup", "LayerType_GallerySegment", "LayerType_Flipbook", "LayerType_SliderGroup", "LayerType_SliderThumbImage", "LayerType_SliderTrackImage" };

        public static MXUIView parse (BinaryReader reader)
        {
            MXUIView view = null;
            if (reader == null) {
                return null;
            }
            //LayerTypeNames[-1] = "LayerType_Unknown";
            var parents = new Dictionary<object, object>();
            if (Encoding.UTF8.GetString(reader.ReadBytes(4)) == "mxcs") {
                while (reader.BaseStream.Position < reader.BaseStream.Length) {
                    var layerType = reader.ReadInt32();
                    var controlId = reader.ReadInt32();
                    var parentId = reader.ReadInt32();
                    object parent = parents.GetOrDefault(parentId);
                    object control = null;
                    switch (LayerTypeNames[layerType]) {
                        case "LayerType_Layer":
                            control = handleImage(reader, parent);
                            break;
                        case "LayerType_View":
                        case "LayerType_SegmentGroup":
                            control = handleView(reader, parent);
                            break;
                        case "LayerType_ButtonGroup":
                            control = handleButtonGroup(reader, parent);
                            break;
                        case "LayerType_ButtonImage":
                            control = handleButtonImage(reader, parent);
                            break;
                        case "LayerType_GallerySegment":
                        case "LayerType_GalleryGroup":
                            control = handleGallery(reader, parent);
                            break;
                        case "LayerType_Flipbook":
                            control = handleFlipbook(reader, parent);
                            break;
                        case "LayerType_SliderGroup":
                            control = handleSliderGroup(reader, parent);
                            break;
                        case "LayerType_SliderThumbImage":
                            control = handleSliderThumbImage(reader, parent);
                            break;
                        case "LayerType_SliderTrackImage":
                            control = handleSliderTrackImage(reader, parent);
                            break;
                        case "LayerType_ScrollingView":
                            control = handleScrollingView(reader, parent);
                            break;
                        case "LayerType_VideoLayer":
                            control = handleVideoLayer(reader, parent);
                            break;
                        default:
                            Console.WriteLine("Unhandled/Unknown layer type: " + layerType);
                            break;
                    }
                    if (control == null) {
                        Console.WriteLine("Couldn\'t finish parsing properly. exiting at position: " + reader.BaseStream.Position);
                        break;
                    }
                    parents[controlId] = control;
                    if (parentId == 0) {
                        view = control as MXUIView;
                    }
                }
            }
            return view;
        }

        private static MXUILayerInfo handleLayerInfo (BinaryReader reader)
        {
            var layerInfo = new MXUILayerInfo();
            layerInfo.name = readString(reader);
            layerInfo.frame = readFrameRect(reader);
            return layerInfo;
        }

        private static MXUIView handleView (BinaryReader reader, Object parent)
        {
            var view = new MXUIView();
            view.layerInfo = handleLayerInfo(reader);
            if (parent == null) {
                readFrameRect(reader);
                reader.ReadBoolean();
            }
            else if (parent is MXUIView) {
                ((MXUIView)parent).insertSubview(view);
            }
            else {
                Console.WriteLine("Error: View\'s parent should be a view. Found: " + getQualifiedClassName(parent));
                Console.WriteLine("Not adding element: \'" + view.layerInfo.name + "\'");
            }
            return view;
        }

        private static MXUIButton handleButtonGroup (BinaryReader reader, Object parent)
        {
            var button = new MXUIButton();
            button.layerInfo = handleLayerInfo(reader);
            button.maskedHitTest = reader.ReadBoolean();
            button.selectorString = readString(reader);
            if (parent is MXUIView) {
                ((MXUIView)parent).insertButton(button);
            }
            else {
                Console.WriteLine("Error: Button\'s parent should be a view. Found: " + getQualifiedClassName(parent));
                Console.WriteLine("Not adding element: \'" + button.layerInfo.name + "\'");
            }
            return button;
        }

        private static MXUIImage handleImage (BinaryReader reader, Object parent)
        {
            var image = new MXUIImage();
            image.layerInfo = handleLayerInfo(reader);
            image.file = readString(reader);
            if (parent is MXUIView) {
                ((MXUIView)parent).insertImage(image);
            }
            else if (parent is MXUIButton || parent is MXUISlider) {}
            else {
                Console.WriteLine("Error: Image\'s parent should be a view. Found: " + getQualifiedClassName(parent));
                Console.WriteLine("Not adding element: \'" + image.layerInfo.name + "\'");
            }
            return image;
        }

        private static Object handleButtonImage (BinaryReader reader, Object parent)
        {
            var image = handleImage(reader, parent);
            var state = reader.ReadInt32();
            if (parent is MXUIButton) {
                ((MXUIButton)parent).imageStates[(MXUIButton.IMAGE_STATE)state] = image;
            }
            else {
                Console.WriteLine("Error: Button Image\'s parent should be a Button. Found: " + getQualifiedClassName(parent));
                Console.WriteLine("Not adding element: \'" + image.layerInfo.name + "\'");
            }
            return image;
        }

        private static MXUIGallery handleGallery (BinaryReader reader, Object parent)
        {
            var gallery = new MXUIGallery();
            gallery.layerInfo = handleLayerInfo(reader);
            if (parent == null) {
                readFrameRect(reader);
                reader.ReadBoolean();
            }
            else if (parent is MXUIView) {
                ((MXUIView)parent).insertSubview(gallery);
            }
            else {
                Console.WriteLine("Error: View\'s parent should be a view. Found: " + getQualifiedClassName(parent));
                Console.WriteLine("Not adding element: \'" + gallery.layerInfo.name + "\'");
            }
            return gallery;
        }

        private static MXUIFlipbook handleFlipbook (BinaryReader reader, Object parent)
        {
            var flipbook = new MXUIFlipbook();
            flipbook.layerInfo = handleLayerInfo(reader);
            readFrameRect(reader);
            reader.ReadBoolean();
            flipbook.sequenceCount = reader.ReadInt32();
            flipbook.imageCount = reader.ReadInt32();
            flipbook.imageBounds = readFrameRect(reader);
            if (parent != null) {
                if (parent is MXUIView) {
                    ((MXUIView)parent).insertSubview(flipbook);
                }
                else {
                    Console.WriteLine("Error: Flipbooks\'s parent should be a view. Found: " + getQualifiedClassName(parent));
                    Console.WriteLine("Not adding element: \'" + flipbook.layerInfo.name + "\'");
                }
            }
            return flipbook;
        }

        private static MXUISlider handleSliderGroup (BinaryReader reader, Object parent)
        {
            var slider = new MXUISlider();
            slider.layerInfo = handleLayerInfo(reader);
            if (parent != null) {
                if (parent is MXUIView) {
                    ((MXUIView)parent).insertSlider(slider);
                }
                else {
                    Console.WriteLine("Error: Slider Group\'s parent should be a view. Found: " + getQualifiedClassName(parent));
                    Console.WriteLine("Not adding element: \'" + slider.layerInfo.name + "\'");
                }
            }
            return slider;
        }

        private static Object handleSliderThumbImage (BinaryReader reader, Object parent)
        {
            var image = handleImage(reader, parent);
            var state = reader.ReadInt32();
            if (parent is MXUISlider) {
                ((MXUISlider)parent).thumbStates[state] = image;
            }
            else {
                Console.WriteLine("Error: Slider Thumb Image\'s parent should be a Slider. Found: " + getQualifiedClassName(parent));
                Console.WriteLine("Not adding element: \'" + image.layerInfo.name + "\'");
            }
            return image;
        }

        private static Object handleSliderTrackImage (BinaryReader reader, Object parent)
        {
            var image = handleImage(reader, parent);
            var state = reader.ReadInt32();
            if (parent is MXUISlider) {
                ((MXUISlider)parent).trackStates[state] = image;
                ((MXUISlider)parent).trackRect = image.layerInfo.frame;
            }
            else {
                Console.WriteLine("Error: Slider Track Image\'s parent should be a Slider. Found: " + getQualifiedClassName(parent));
                Console.WriteLine("Not adding element: \'" + image.layerInfo.name + "\'");
            }
            return image;
        }

        private static MXUIScrollView handleScrollingView (BinaryReader reader, Object parent)
        {
            var scrollView = new MXUIScrollView();
            scrollView.layerInfo = handleLayerInfo(reader);
            scrollView.contentFrame = readFrameRect(reader);
            scrollView.zoomContent = reader.ReadBoolean();
            if (parent == null) {}
            else if (parent is MXUIView) {
                ((MXUIView)parent).insertSubview(scrollView);
            }
            else {
                Console.WriteLine("Error: ScrollView\'s parent should be a view. Found: " + getQualifiedClassName(parent));
                Console.WriteLine("Not adding element: \'" + scrollView.layerInfo.name + "\'");
            }
            return scrollView;
        }

        private static MXUIVideo handleVideoLayer (BinaryReader reader, Object parent)
        {
            var video = new MXUIVideo();
            video.layerInfo = handleLayerInfo(reader);
            video.filename = readString(reader);
            if (parent is MXUIView) {
                ((MXUIView)parent).insertSubview(video);
            }
            else {
                Console.WriteLine("Error: Videos\'s parent should be a view. Found: " + getQualifiedClassName(parent));
                Console.WriteLine("Not adding element: \'" + video.layerInfo.name + "\'");
            }
            return video;
        }

        private static Rect readFrameRect (BinaryReader reader)
        {
            return new Rect(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        private static String readString (BinaryReader reader)
        {
            var startPos = (int)reader.BaseStream.Position;
            while (reader.BaseStream.Position < reader.BaseStream.Length)
                if (reader.ReadByte() == 0)
                    break;
            var length = (int)reader.BaseStream.Position - startPos;
            reader.BaseStream.Position = startPos;
            return Encoding.UTF8.GetString(reader.ReadBytes(length)).TrimEnd('\0');
        }

        private static string getQualifiedClassName (object o)
        {
            return o != null ? o.GetType().Name : "";
        }
    }

    public static class Exts
    {
        public static TValue GetOrDefault<TKey, TValue> (this IDictionary<TKey, TValue> @this, TKey key, TValue def = default(TValue))
        {
            TValue value;
            return @this.TryGetValue(key, out value) ? value : def;
        }
    }
}