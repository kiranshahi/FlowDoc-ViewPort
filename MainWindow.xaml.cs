using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace FlowDoc_ViewPort
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BindData();
        }

        private void BindData()
        {
            string text = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin rutrum hendrerit sapien at hendrerit. Pellentesque posuere eu mauris nec
                            vulputate. Phasellus consectetur orci vel arcu fermentum ullamcorper. Aliquam erat volutpat. Aenean sem ligula, varius eu auctor in,
                            vestibulum in dui. Nam massa mauris, aliquam ut ullamcorper ac, suscipit at erat. Curabitur varius purus nec dapibus bibendum. Aenean
                            fringilla lacus non lectus consectetur auctor. Ut fringilla, arcu quis molestie ultrices, erat velit euismod purus, in pretium nibh metus
                            pulvinar tellus.

                            \n\n

                            Curabitur sed suscipit felis. Sed nisi diam, placerat ut tempus sed, ultricies vel magna. Aliquam et dolor et justo tempor ornare eleifend id
                            metus. Cras nec sollicitudin lectus, sed volutpat orci. Nunc eget mollis orci. Etiam sed lorem eget dui blandit finibus. Etiam ante libero,
                            aliquam tincidunt turpis non, eleifend porttitor risus. Vivamus sit amet est quis quam rhoncus ultrices vitae ac est. Quisque consectetur
                            purus quis aliquet ullamcorper. Nullam blandit risus felis, sed pretium odio tempor sed. Donec pharetra risus et convallis vulputate.
                            Maecenas imperdiet felis velit, vitae gravida ante pellentesque ut. Fusce eget est quis arcu accumsan dapibus eget ut odio. In a sagittis
                            quam.

                            \n\n
                            
                            Suspendisse iaculis eleifend odio et euismod. Nam dapibus consequat quam, et malesuada augue tincidunt dictum. Mauris condimentum
                            massa et maximus suscipit. Mauris aliquam ipsum at facilisis laoreet. Donec vestibulum dui sit amet pellentesque maximus. Aenean urna
                            metus, semper id ante vitae, lobortis mollis nibh. Aenean ut tristique massa, eu tempor justo. Quisque tortor dui, dignissim et gravida id,
                            aliquet sit amet orci. Donec facilisis leo nec egestas efficitur. Nulla fringilla hendrerit feugiat.

                            \n\n

                            Integer egestas vel ex vel sollicitudin. Aliquam porttitor mauris nibh, eu porttitor justo dictum vitae. Etiam venenatis tincidunt elit, ut
                            laoreet tortor tincidunt sit amet. Aenean eget urna nec ligula finibus tincidunt et a lectus. Morbi quis imperdiet neque. Sed ut libero vitae
                            sem convallis molestie. Pellentesque posuere tellus et convallis facilisis. Etiam porta turpis vel libero laoreet euismod. Orci varius natoque
                            penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec leo turpis, consequat a erat non, maximus gravida ipsum.
                            Aliquam erat volutpat. Proin non metus pretium, varius enim vel, imperdiet nunc.

                            \n\n

                            Pellentesque dictum, enim ac ullamcorper dignissim, tellus mauris vehicula erat, sed scelerisque lorem urna ac turpis. Donec rhoncus ut
                            leo ut tincidunt. Nam posuere erat in tristique sodales. Aliquam at massa at ante pretium pharetra. Proin feugiat blandit enim a eleifend.
                            Nulla ornare lorem rutrum purus vulputate venenatis. Etiam ac lectus non risus finibus gravida id id turpis. Nullam nec vulputate lectus.
                            Proin ac augue vel velit facilisis cursus vitae quis lectus. Duis placerat vulputate lacus, id mattis turpis faucibus nec. Aliquam vitae tempus
                            purus. Sed feugiat elementum velit, ac interdum sem luctus sit amet. Nunc tristique tincidunt porttitor. Sed nec nunc ut tellus pretium
                            semper.";
            for (int i = 1; i <= 100; i++)
            {
                Paragraph para = new Paragraph() { Name = $"para{i}" };
                para.Inlines.Add(new TextBlock { Name = $"page{i}", Text = text, TextWrapping = TextWrapping.Wrap });

                Section section = new Section() { Name = $"sec{i}" };
                section.Blocks.Add(para);

                flowdoc1.Blocks.Add(section);
            }

        }

        private void FlowDoc_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer sv = flowDoc.Template.FindName("PART_ContentHost", flowDoc) as ScrollViewer;
            sv.ScrollChanged += Sv_ScrollChanged;
        }


        private void Sv_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            try
            {
                List<Visual> viz = GetVisualTree(flowDoc).ToList();

                var fpoint = flowDoc.PointToScreen(new Point(0, 0));
                var fHeight = flowDoc.ActualHeight;

                //var minYPoint = fpoint.Y;
                //var maxYPoint = fpoint.Y + fHeight;

                var minYPoint = 0;
                var maxYPoint = fpoint.Y;

                string CurrentItemName = string.Empty;

                Rect frect = new Rect(fpoint.X, fpoint.Y, flowDoc.ActualWidth, flowDoc.ActualHeight);

                foreach (var item in viz)
                {
                    var pointInScreen = item.PointToScreen(new Point(0, 0));

                    var currentItem = item as TextBlock;

                    var point = new Rect(pointInScreen.X, pointInScreen.Y, currentItem.ActualWidth, currentItem.ActualHeight);

                    if (point.IntersectsWith(frect))
                    {
                        CurrentItemName += $"Visibile Control: {currentItem.Name} x: {point.X} y: {point.Y} \n";
                        //break;
                    }
                }

                itemsInView.Content = CurrentItemName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private IEnumerable<Visual> GetVisualTree(Visual v)
        {
            var t = VisualTreeHelper.GetChildrenCount(v);
            for (var i = 0; i < t; i++)
            {
                var c = (Visual)VisualTreeHelper.GetChild(v, i);
                foreach (var x in GetVisualTree(c)) yield return x;
            }

            if (v is TextBlock)
            {
                yield return v;
            }
        }
    }
}
