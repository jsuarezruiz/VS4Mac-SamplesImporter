using MonoDevelop.Components;
using MonoDevelop.Ide.Fonts;
using Xwt;

namespace VS4Mac.SamplesImporter.Controls
{
    public class TagWidget : RoundedFrameBox
    {
        HBox _tagBox;
        Label _textLabel;
        string _text;

        public TagWidget()
        {
            Init();
            BuildGui();
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                UpdateText();
            }
        }

        void Init()
        {
            CornerRadiusTopLeft = 6;
            CornerRadiusTopRight = 6;
            CornerRadiusBottomLeft = 6;
            CornerRadiusBottomRight = 6;
            BorderWidth = 1;
            InnerBackgroundColor = MonoDevelop.Ide.Gui.Styles.BaseSelectionBackgroundColor;
            MinWidth = 100;
            Margin = new WidgetSpacing(0, 0, 6, 0);

            _tagBox = new HBox();

            _textLabel = new Label
            {
                Font = Xwt.Drawing.Font.FromName(FontService.SansFontName).WithScaledSize(0.9),
                TextColor = MonoDevelop.Ide.Gui.Styles.BaseSelectionTextColor,
                Ellipsize = EllipsizeMode.End
            };
        }

        void BuildGui()
        {
            _tagBox.PackStart(_textLabel, true, true);

            Content = _tagBox;
        }

        void UpdateText()
        {
            if (string.IsNullOrEmpty(_text))
            {
                return;
            }

            _textLabel.Text = _text;
            _textLabel.TooltipText = Text;

            var characters = _text.Length;

            if (characters > 48)
            {
                _textLabel.WidthRequest = 200;
            }
        }
    }
}