using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ReactNative.UIManager
{
    /// <summary>
    /// Class providing border management API for  view managers.
    /// </summary>
    public abstract class BorderedViewParentManager<TFrameworkElement> : ViewParentManager<BorderedContentControl>
        where TFrameworkElement : FrameworkElement
    {
        private static readonly Brush s_defaultBorderBrush = new SolidColorBrush(Colors.Black);

        /// <summary>
        /// Sets the border radius of the view.
        /// </summary>
        /// <param name="view">The view panel.</param>
        /// <param name="radius">The border radius value.</param>
        [ReactProperty("borderRadius")]
        public void SetBorderRadius(BorderedContentControl view, double? radius)
        {
            view.SetBorderRadius(radius);
        }

        /// <summary>
        /// Sets the background color of the view.
        /// </summary>
        /// <param name="view">The view panel.</param>
        /// <param name="color">The masked color value.</param>
        [ReactProperty(ViewProperties.BackgroundColor)]
        public void SetBackgroundColor(BorderedContentControl view, uint? color)
        {
            view.SetBackgroundColor(color);
        }

        /// <summary>
        /// Set the border color of the view.
        /// </summary>
        /// <param name="view">The view panel.</param>
        /// <param name="color">The color hex code.</param>
        [ReactProperty("borderColor", CustomType = "Color")]
        public void SetBorderColor(BorderedContentControl view, uint? color)
        {
            view.SetBorderColor(color);
        }

        /// <summary>
        /// Sets the border thickness of the view.
        /// </summary>
        /// <param name="view">The view panel.</param>
        /// <param name="index">The property index.</param>
        /// <param name="width">The border width in pixels.</param>
        [ReactPropertyGroup(
            ViewProperties.BorderWidth,
            ViewProperties.BorderLeftWidth,
            ViewProperties.BorderRightWidth,
            ViewProperties.BorderTopWidth,
            ViewProperties.BorderBottomWidth,
            DefaultDouble = double.NaN)]
        public void SetBorderWidth(BorderedContentControl view, int index, double? width)
        {
            view.SetBorderWidth(ViewProperties.BorderSpacingTypes[index], width);
        }

        /// <summary>
        /// Sets whether the view is collapsible.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="collapsible">The flag.</param>
        [ReactProperty(ViewProperties.Collapsible)]
        public void SetCollapsible(BorderedContentControl view, bool collapsible)
        {
            // no-op: it's here only so that "collapsable" property is exported to JS. The value is actually
            // handled in NativeViewHierarchyOptimizer
        }

        /// <summary>
        /// Adds a child at the given index.
        /// </summary>
        /// <param name="parent">The parent view.</param>
        /// <param name="child">The child view.</param>
        /// <param name="index">The index.</param>
        public sealed override void AddView(BorderedContentControl parent, FrameworkElement child, int index)
        {
            var inner = GetInnerElement(parent);
            AddView(inner, child, index);
        }

        /// <summary>
        /// Gets the number of children in the view parent.
        /// </summary>
        /// <param name="parent">The view parent.</param>
        /// <returns>The number of children.</returns>
        public sealed override int GetChildCount(BorderedContentControl parent)
        {
            var inner = GetInnerElement(parent);
            return GetChildCount(inner);
        }

        /// <summary>
        /// Gets the child at the given index.
        /// </summary>
        /// <param name="parent">The parent view.</param>
        /// <param name="index">The index.</param>
        /// <returns>The child view.</returns>
        public override FrameworkElement GetChildAt(BorderedContentControl parent, int index)
        {
            var inner = GetInnerElement(parent);
            return GetChildAt(inner, index);
        }

        /// <summary>
        /// Removes the child at the given index.
        /// </summary>
        /// <param name="parent">The view parent.</param>
        /// <param name="index">The index.</param>
        public override void RemoveChildAt(BorderedContentControl parent, int index)
        {
            var inner = GetInnerElement(parent);
            RemoveChildAt(inner, index);
        }

        /// <summary>
        /// Removes all children from the view parent.
        /// </summary>
        /// <param name="parent">The view parent.</param>
        public override void RemoveAllChildren(BorderedContentControl parent)
        {
            var inner = GetInnerElement(parent);
            RemoveAllChildren(inner);
        }

        /// <summary>
        /// Creates a new view instance of type <see cref="Border"/>.
        /// </summary>
        /// <param name="reactContext">The react context.</param>
        /// <returns>The view instance.</returns>
        protected sealed override BorderedContentControl CreateViewInstance(ThemedReactContext reactContext)
        {
            var inner = CreateInnerElement(reactContext);
            return new BorderedContentControl(inner)
            {
                IsTabStop = true,
            };
        }

        /// <summary>
        /// Creates a new view instance of type <typeparamref name="TFrameworkElement"/>.
        /// </summary>
        /// <param name="reactContext">The react context.</param>
        /// <returns>The view instance.</returns>
        protected abstract TFrameworkElement CreateInnerElement(ThemedReactContext reactContext);

        /// <summary>
        /// Adds a child at the given index.
        /// </summary>
        /// <param name="parent">The parent view.</param>
        /// <param name="child">The child view.</param>
        /// <param name="index">The index.</param>
        protected abstract void AddView(TFrameworkElement parent, FrameworkElement child, int index);

        /// <summary>
        /// Gets the number of children in the view parent.
        /// </summary>
        /// <param name="parent">The view parent.</param>
        /// <returns>The number of children.</returns>
        protected abstract int GetChildCount(TFrameworkElement parent);

        /// <summary>
        /// Gets the child at the given index.
        /// </summary>
        /// <param name="parent">The parent view.</param>
        /// <param name="index">The index.</param>
        /// <returns>The child view.</returns>
        protected abstract FrameworkElement GetChildAt(TFrameworkElement parent, int index);

        /// <summary>
        /// Removes the child at the given index.
        /// </summary>
        /// <param name="parent">The view parent.</param>
        /// <param name="index">The index.</param>
        protected abstract void RemoveChildAt(TFrameworkElement parent, int index);

        /// <summary>
        /// Removes all children from the view parent.
        /// </summary>
        /// <param name="parent">The view parent.</param>
        protected abstract void RemoveAllChildren(TFrameworkElement parent);

        /// <summary>
        /// Get the inner element of the border.
        /// </summary>
        /// <param name="parent">The parent view.</param>
        /// <returns>The inner element.</returns>
        protected TFrameworkElement GetInnerElement(BorderedContentControl parent)
        {
            return (TFrameworkElement)parent.Content;
        }
    }
}
