﻿using Newtonsoft.Json.Linq;
using ReactNative.Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ReactNative.UIManager.LayoutAnimation
{
    /// <summary>
    /// Class responsible for animation layout changes.
    /// </summary>
    public class LayoutAnimationManager
    {
        private const string CONFIG_PROP_DURATION = "duration";
        public const string CONFIG_PROP_ACTION_CREATE = "create";
        private const string CONFIG_PROP_ACTION_UPDATE = "update";
        private Dictionary<AnimationState, StoryboardAnimation> _AnimationLayoutDictionary;
        private bool _ShouldAnimateLayout;

        /// <summary>
        /// Setup the initial settings of the initial and follow-on <see cref="Storyboard"/>(s).
        /// </summary>
        /// <param name="config">The JSON config of the animation.</param>
        public void InitializeFromConfig(JObject config)
        {
            var durationToken = default(JToken);
            var actionTypeCreateToken = default(JToken);
            var actionTypeUpdateToken = default(JToken);
            var globalDuration = default(int);

            _AnimationLayoutDictionary = new Dictionary<AnimationState, StoryboardAnimation>()
            {
                { AnimationState.create, new LayoutCreateAnimation() },
                { AnimationState.update, new LayoutUpdateAnimation() },
            };

            if (config == null)
            {
                Reset();
                return;
            }

            _ShouldAnimateLayout = false;
            globalDuration = config.TryGetValue(CONFIG_PROP_DURATION, out durationToken) ? durationToken.ToObject<int>() : 0;

            if (config.TryGetValue(CONFIG_PROP_ACTION_CREATE, out actionTypeCreateToken))
            {
                this.Storyboard(AnimationState.create).InitializeFromConfig(actionTypeCreateToken.ToObject<JObject>(), globalDuration);
                _ShouldAnimateLayout = true;
            }

            if (config.TryGetValue(CONFIG_PROP_ACTION_UPDATE, out actionTypeUpdateToken))
            {
                this.Storyboard(AnimationState.update).InitializeFromConfig(actionTypeUpdateToken.ToObject<JObject>(), globalDuration);
                _ShouldAnimateLayout = true;
            }
        }

        /// <summary>
        /// Returns the <see cref="StoryboardAnimation"/> to animate based on the <see cref="AnimationState"/>. Currently there is one for animating the initial state
        /// of a <see cref="FrameworkElement"/> and another for updating the layout change(i.e. update). 
        /// </summary>
        /// <param name="type">The desired <see cref="AnimationState"/>.</param>
        /// <returns>Thre <see cref="StoryboardAnimation"/> reference.</returns>
        public StoryboardAnimation Storyboard(AnimationState type)
        {
            var animation = default(StoryboardAnimation);

            if (_AnimationLayoutDictionary.TryGetValue(type, out animation))
            {
                return animation;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Determines if <see cref="FrameworkElement"/> should apply the animation <see cref="StoryBoard"/>.
        /// </summary>
        /// <param name="view">The view to animate.</param>
        /// <returns></returns>
        public bool ShouldAnimateLayout(FrameworkElement view) { return _ShouldAnimateLayout && view.Parent != null; }

        public void Reset()
        {
            this.Storyboard(AnimationState.create).Reset();
            this.Storyboard(AnimationState.update).Reset();
            _ShouldAnimateLayout = false;
        }

        /// <summary>
        /// Create the animation <see cref="Storyboard"/> and kick off the binded <see cref="Timeline"/>(s). 
        /// </summary>
        /// <param name="view">The native view to animate.</param>
        /// <param name="x">The new X position to animate to.</param>
        /// <param name="y">The new Y position to animate to.</param>
        /// <param name="width">The new width that the <see cref="FrameworkElement"/> needs to transform to.</param>
        /// <param name="height">The new height that the <see cref="FrameworkElement"/> needs to transform to.</param>
        public void ApplyLayoutUpdate(FrameworkElement view, int x, int y, int width, int height)
        {
            DispatcherHelpers.AssertOnDispatcher();

            var animationState = view.ActualWidth == 0 || view.ActualHeight == 0 ? AnimationState.create : AnimationState.update;
            var storyboard = this.Storyboard(animationState).CreateAnimation(view, x, y, width, height);

            if(storyboard != null)
            {
                storyboard.Begin();
            }
        }
    }
}
