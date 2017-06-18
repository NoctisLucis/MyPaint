using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.IO;
using ShapesLib;
using MyPaint;
using MyPaint.Adorners;

namespace MyPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AdornerLayer adlayer;
        Adorner adCanvas;
        BitmapImage oriimage;
        bool colorBit = false; // bat bit tranh loi khoi tao mau

        Random random = new Random();
        Point startPoint, endPoint;
        DoubleCollection dashes;

        public MainWindow()
        {
            InitializeComponent();
            SetSize();
        }

        private void paintCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(paintCanvas);

            if (type == DrawType.CLine || type == DrawType.CRectangle || type == DrawType.CSquare
                    || type == DrawType.CEllipse || type == DrawType.CCircle || type == DrawType.CHeart || type == DrawType.CArrow || type == DrawType.CStar)
                DrawShape_MouseDown(e);

            if (type == DrawType.CSelect)
            {
                DrawSelect_MouseDown();
            }

            if (type == DrawType.CText)
            {
                DrawText_MouseDown();
            }

            if (type == DrawType.CFill)
            {
                if (adlayer != null)
                    ChangeImage();
                Set_Fill();
            }


        }

        private void paintCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (startPoint == endPoint)
                return;

            if (type == DrawType.CLine || type == DrawType.CRectangle || type == DrawType.CSquare
                   || type == DrawType.CEllipse || type == DrawType.CCircle || type == DrawType.CHeart || type == DrawType.CArrow || type == DrawType.CStar)
                DrawShape_MouseUp(sender);

            if (type == DrawType.CSelect)
            {
                DrawSelect_MouseUp();
            }

            if (type == DrawType.CText)
            {
                DrawText_MouseUp();
            }
        }

        private void paintCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (type == DrawType.CLine || type == DrawType.CRectangle || type == DrawType.CSquare
                    || type == DrawType.CEllipse || type == DrawType.CCircle || type == DrawType.CHeart || type == DrawType.CArrow || type == DrawType.CStar)
                DrawShape_MouseMove(e);

            if (type == DrawType.CSelect)
            {
                endPoint = e.GetPosition(paintCanvas);
                DrawSelect_MouseMove();
            }

            if (type == DrawType.CText)
            {
                endPoint = e.GetPosition(paintCanvas);
                DrawText_MouseMove();
            }
        }

        private void MyPaint_Loaded(object sender, RoutedEventArgs e)
        {
            Line.IsChecked = true;
            style1.IsChecked = true;
            ChangeImage();

        }

        // Shape -------------------------------------------------------------

        #region Xu Li Shape
        CShape shape = new CLine();

        int Thickness;
        DrawType type;
        LinearGradientBrush fiveColorLGB;
        GradientStop GS1, GS2;
        private void DrawShape_MouseDown(MouseButtonEventArgs e)
        {
            ChangeImage();
            if (adlayer != null)
                shape.RemoveAdorner(adlayer);
            colorBit = true;
            Thickness = Int32.Parse(borderSizeComboBox.SelectedItem.ToString());
           
            switch (type)
            {
                case DrawType.CLine:
                    shape = new CLine(); break;
                case DrawType.CRectangle:
                    shape = new CRectangle(); break;
                case DrawType.CSquare:
                    shape = new CSquare(); break;
                case DrawType.CEllipse:
                    shape = new CEllipse(); break;
                case DrawType.CCircle:
                    shape = new CCircle(); break;
                case DrawType.CHeart:
                    shape = new CHeart(); break;
                case DrawType.CStar:
                    shape = new CStar(); break;
                case DrawType.CArrow:
                    shape = new CArrow(); break;
            }

            fiveColorLGB = new LinearGradientBrush();
            fiveColorLGB.StartPoint = new Point(0, 0);
            fiveColorLGB.EndPoint = new Point(1, 0);

            GS1 = new GradientStop();
            GS1.Color = (Color)colorPicker2.SelectedColor;
            GS1.Offset = 1 - (float)Offset.Value;
            fiveColorLGB.GradientStops.Add(GS1);

            GS2 = new GradientStop();
            GS2.Color = (Color)colorPicker3.SelectedColor;
            GS2.Offset = 1;
            fiveColorLGB.GradientStops.Add(GS2);

            shape.m_Fill = fiveColorLGB;
            shape.m_Stroke = new SolidColorBrush((Color)colorPicker1.SelectedColor);
            shape.m_ThinkNess = Thickness;
            shape.m_Dash = dashes;
            shape.DrawDown(paintCanvas, startPoint);
        }

        private void DrawShape_MouseMove(MouseEventArgs e)
        {          
           endPoint = e.GetPosition(paintCanvas);

            if (e.LeftButton == MouseButtonState.Released)
                return;
            shape.DrawMove(paintCanvas, endPoint);
        }

        private void DrawShape_MouseUp(object sender)
        {
           
            //if (startPoint.X == endPoint.X && startPoint.Y == endPoint.Y)
            //    return;
            ((Canvas)sender).ReleaseMouseCapture();
            shape.AddAdorner(adlayer);
            if (paintCanvas.Children.Count > 0)
            {
                Undo.IsEnabled = true;
            }
        }

        private void Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (adlayer != null)
                shape.Remove(paintCanvas);

        }

        #endregion

        // Select -------------------------------------------------------------

        #region Xu Li Select
        Image currentSelectedImage = null;
        Rectangle SelectRectangle = null;

        private void DrawSelect_MouseDown()
        {
            if (SelectRectangle != null)
            {
                paintCanvas.Children.Remove(SelectRectangle);
                SelectRectangle = null;
            }

            if (currentSelectedImage != null)
            {
                //CImage RemoveAdorner: equivalent drawingSurface.RemoveElementAdorner(currentSelectedImage);
                if (adlayer != null)
                {
                    Adorner[] adorners = adlayer.GetAdorners(currentSelectedImage);
                    if (adorners != null)
                    {
                        foreach (var adorner in adorners)
                            adlayer.Remove(adorner);
                    }
                }
                if (cop == 1)
                    adlayer.Remove(imagecopy_adorner);
                cop = 0;
                currentSelectedImage = null;

            }
            RemoveRectAdorner(copyImage);
            if (SelectRectangle == null)
                paintCanvas.Children.Add(SelectRectangle = new Rectangle()
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    StrokeDashArray = new DoubleCollection() { 1, 2 },
                });
            Canvas.SetLeft(SelectRectangle, startPoint.X);
            Canvas.SetTop(SelectRectangle, startPoint.Y);
        }

        private void DrawSelect_MouseMove()
        {
            if (SelectRectangle == null) return;
            Canvas.SetLeft(SelectRectangle, Math.Max(Math.Min(startPoint.X, endPoint.X), 0));
            Canvas.SetTop(SelectRectangle, Math.Max(Math.Min(startPoint.Y, endPoint.Y), 0));
            SelectRectangle.Width = Math.Min(Math.Abs(startPoint.X - endPoint.X), paintCanvas.Width - Canvas.GetLeft(SelectRectangle));
            SelectRectangle.Height = Math.Min(Math.Abs(startPoint.Y - endPoint.Y), paintCanvas.Height - Canvas.GetTop(SelectRectangle));
        }

        private void DrawSelect_MouseUp()
        {
            if (SelectRectangle != null && currentSelectedImage == null)
            {
                if (SelectRectangle.Height < 2 || SelectRectangle.Width < 2)
                {
                    paintCanvas.Children.Remove(SelectRectangle);
                    SelectRectangle = null;
                }
                else
                {
                    paintCanvas.Children.Remove(SelectRectangle);
                    currentSelectedImage = CanvasUltilities.Crop(paintCanvas, Canvas.GetLeft(SelectRectangle), Canvas.GetTop(SelectRectangle), SelectRectangle.Width, SelectRectangle.Height);

                    //Add replace rectangle to Hide view behind cropped region
                    Rectangle replace = new Rectangle()
                    {
                        Width = SelectRectangle.Width,
                        Height = SelectRectangle.Height,
                        Stroke = Brushes.Transparent,
                        StrokeThickness = 0,
                        Fill = Brushes.White
                    };
                    Canvas.SetLeft(replace, Canvas.GetLeft(SelectRectangle));
                    Canvas.SetTop(replace, Canvas.GetTop(SelectRectangle));
                    paintCanvas.Children.Add(replace);


                    currentSelectedImage.Stretch = Stretch.Fill;
                    currentSelectedImage.StretchDirection = StretchDirection.Both;
                    Canvas.SetLeft(currentSelectedImage, Canvas.GetLeft(SelectRectangle));
                    Canvas.SetTop(currentSelectedImage, Canvas.GetTop(SelectRectangle));
                    currentSelectedImage.Width = currentSelectedImage.Source.Width;
                    currentSelectedImage.Height = currentSelectedImage.Source.Height;

                    paintCanvas.Children.Add(currentSelectedImage);
                    Adorner image_adorner = new SelectAdorner(currentSelectedImage);
                    adlayer.Add(image_adorner);
                }
            }
        }
        #endregion

        // Text -------------------------------------------------------------

        #region Xu Li Text
        TextBox tbox = null;
        Rectangle m_Rect = null;

        private Image paintCanvas_CropAt(int x, int y, double w, double h)
        {
            return CanvasUltilities.Crop(paintCanvas, x, y, w, h);
        }

        private void DrawText_MouseDown()
        {
            if (m_Rect != null)
            {
                paintCanvas.Children.Remove(m_Rect);
                m_Rect = null;
            }

            if (tbox != null)
            {
                tbox.BorderBrush = Brushes.Transparent;
                tbox.Focusable = false;
                if (adlayer != null)
                {
                    Adorner[] adorners = adlayer.GetAdorners(tbox);
                    if (adorners != null)
                    {
                        foreach (var adorner in adorners)
                            adlayer.Remove(adorner);
                    }
                }
                

                Image TextImage = new Image();
                TextImage = paintCanvas_CropAt((int)Canvas.GetLeft(tbox), (int)Canvas.GetTop(tbox), tbox.Width, tbox.Height);
                Canvas.SetLeft(TextImage, Canvas.GetLeft(tbox));
                Canvas.SetTop(TextImage, Canvas.GetTop(tbox));
                paintCanvas.Children.Add(TextImage);
                paintCanvas.Children.Remove(tbox);
            }

            if (m_Rect == null)
            {
                paintCanvas.Children.Add(m_Rect = new Rectangle()
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    StrokeDashArray = new DoubleCollection() { 2, 4 },
                });
            }
            Canvas.SetLeft(m_Rect, startPoint.X);
            Canvas.SetTop(m_Rect, startPoint.Y);
        }

        private void DrawText_MouseMove()
        {
            if (m_Rect == null) return;
            Canvas.SetLeft(m_Rect, Math.Max(Math.Min(startPoint.X, endPoint.X), 0));
            Canvas.SetTop(m_Rect, Math.Max(Math.Min(startPoint.Y, endPoint.Y), 0));
            m_Rect.Width = Math.Min(Math.Abs(startPoint.X - endPoint.X), paintCanvas.Width - Canvas.GetLeft(m_Rect));
            m_Rect.Height = Math.Min(Math.Abs(startPoint.Y - endPoint.Y), paintCanvas.Height - Canvas.GetTop(m_Rect));
        }

        private void DrawText_MouseUp()
        {

            if (m_Rect != null)
            {
                if (m_Rect.Height < 2 || m_Rect.Width < 2)
                {
                    paintCanvas.Children.Remove(m_Rect);
                    m_Rect = null;

                }
                else
                {
                    paintCanvas.Children.Remove(m_Rect);
                    tbox = new TextBox()
                    {
                        Width = m_Rect.Width,
                        Height = m_Rect.Height,
                        TextWrapping = TextWrapping.Wrap,
                        BorderBrush = Brushes.Transparent,
                        Foreground = new SolidColorBrush((Color)colorText.SelectedColor),
                        Background = new SolidColorBrush((Color)colorBGText.SelectedColor),
                    };

                    Canvas.SetLeft(tbox, Canvas.GetLeft(m_Rect));
                    Canvas.SetTop(tbox, Canvas.GetTop(m_Rect));
                    paintCanvas.Children.Add(tbox);

                    Adorner txtbox_adorner = new TextBoxAdorner(tbox);
                    adlayer.Add(txtbox_adorner);
                    tbox.Focus();
                }
            }
        }

        private void SetSize()
        {
            fontsComboBox.ItemsSource = Fonts.SystemFontFamilies;
            fontsComboBox.SelectedIndex = 0;

            for (double i = 8; i < 48; i += 2)
            {
                fontSizeComboBox.Items.Add(i);
            }
            borderSizeComboBox.Items.Add(1);
            for (double i = 2; i < 48; i += 2)
            {
                borderSizeComboBox.Items.Add(i);
            }
            fontSizeComboBox.SelectedIndex = 4;
            borderSizeComboBox.SelectedIndex = 1;
        }

        private void fontsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tbox == null) return;
            tbox.FontFamily = (FontFamily)fontsComboBox.SelectedItem;
            tbox.BorderBrush = Brushes.Transparent;
            tbox.Focus();
        }

        private void fontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (tbox == null) return;
            tbox.FontSize = Int32.Parse(fontSizeComboBox.SelectedItem.ToString());
            tbox.BorderBrush = Brushes.Transparent;
            tbox.Focus();
        }

        private void colorText_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (tbox == null) return;
            tbox.Foreground = new SolidColorBrush((Color)colorText.SelectedColor);
            tbox.BorderBrush = Brushes.Transparent;
            tbox.Focus();
        }

        private void colorBackGround_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (tbox == null) return;
            tbox.Background = new SolidColorBrush((Color)colorBGText.SelectedColor);
            tbox.BorderBrush = Brushes.Transparent;
            tbox.Focus();
        }
        #endregion


        // Dash --------------------------------------------------------

        private void Size_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void style1_Checked(object sender, RoutedEventArgs e)
        {
            dashes = new DoubleCollection();
            dashes.Clear();
            if (adlayer != null)
                shape.ChangeDash(dashes);
        }

        private void style2_Checked(object sender, RoutedEventArgs e)
        {
            dashes = new DoubleCollection();
            dashes.Clear();
            dashes.Add(1);
            dashes.Add(1);
            if (adlayer != null)
                shape.ChangeDash(dashes);
        }

        private void style3_Checked(object sender, RoutedEventArgs e)
        {
            dashes = new DoubleCollection();
            dashes.Clear();
            dashes.Add(4);
            dashes.Add(3);
            if (adlayer != null)
                shape.ChangeDash(dashes);
        }

        private void style4_Checked(object sender, RoutedEventArgs e)
        {
            dashes = new DoubleCollection();
            dashes.Clear();
            dashes.Add(1);
            dashes.Add(2);
            dashes.Add(4);
            dashes.Add(2);
            if (adlayer != null)
                shape.ChangeDash(dashes);
        }


        //Save load -----------------------------------------------------

        private BitmapImage CreateBitmapImage(Canvas canvas)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(canvas);
            double dpi = 96d;

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(canvas);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }


            rtb.Render(dv);
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            pngEncoder.Save(ms);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }

        private void ChangeImage()
        {
            nvt++;
            nImage++;
            if (nImage == nvt)
                listBitmapImage.Add(oriimage);
            nImage = nvt;
            oriimage = CreateBitmapImage(paintCanvas);

            listBitmapImage[nImage - 1] = oriimage;

            ImageBrush image = new ImageBrush();
            image.ImageSource = oriimage;
            paintCanvas.Children.Clear();

            if (adCanvas == null || adlayer == null) return;


            paintCanvas.Height = image.ImageSource.Height;
            paintCanvas.Width = image.ImageSource.Width;

            adlayer.Remove(adCanvas);
            adlayer = AdornerLayer.GetAdornerLayer(paintCanvas);
            adCanvas = new CanvasAdorner(paintCanvas);
            adlayer.Add(adCanvas);
            Undo.IsEnabled = true;
            paintCanvas.Background = image;
        }

        private void CreateSaveBitmap(Canvas canvas, string filename)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(canvas);
            double dpi = 96d;


            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);


            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(canvas);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            rtb.Render(dv);
            string type = filename.Substring(filename.LastIndexOf('.'));
            if (type == ".png")
            {
                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
                try
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();

                    pngEncoder.Save(ms);
                    ms.Close();

                    System.IO.File.WriteAllBytes(filename, ms.ToArray());
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            if (type == ".jpg")
            {
                BitmapEncoder jpgEncoder = new JpegBitmapEncoder();
                jpgEncoder.Frames.Add(BitmapFrame.Create(rtb));
                try
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();

                    jpgEncoder.Save(ms);
                    ms.Close();

                    System.IO.File.WriteAllBytes(filename, ms.ToArray());
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                BitmapEncoder bmpEncoder = new BmpBitmapEncoder();
                bmpEncoder.Frames.Add(BitmapFrame.Create(rtb));
                try
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();

                    bmpEncoder.Save(ms);
                    ms.Close();

                    System.IO.File.WriteAllBytes(filename, ms.ToArray());
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void saveas_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Untitled"; // Default file name
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "PNG Image|*.png|JPEG Image|*.jpg|BMP Image|*.bmp"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                CreateSaveBitmap(paintCanvas, filename);
            }
        }

        private void load_Click(object sender, System.Windows.RoutedEventArgs e)
        {     
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Untitled";
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Image|*.png|JPEG Image|*.jpg|BMP Image|*.bmp";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                
                string filename = dlg.FileName;
                ImageBrush image = new ImageBrush();
                //image.ImageSource = new BitmapImage(new Uri(@filename, UriKind.Relative));
                oriimage = new BitmapImage(new Uri(@filename, UriKind.Relative));
                image.ImageSource = oriimage;
                paintCanvas.Children.Clear();

                if (adCanvas == null || adlayer == null) return;


                paintCanvas.Height = image.ImageSource.Height;
                paintCanvas.Width = image.ImageSource.Width;

                adlayer.Remove(adCanvas);
                adlayer = AdornerLayer.GetAdornerLayer(paintCanvas);
                adCanvas = new CanvasAdorner(paintCanvas);
                adlayer.Add(adCanvas);
                
                paintCanvas.Background = image;
            }
        }

        private void quit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void new_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            paintCanvas.Children.Clear();
        }

        private void paintCanvas_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            adlayer = AdornerLayer.GetAdornerLayer(sender as Canvas);
            adCanvas = new CanvasAdorner(paintCanvas);
            adlayer.Add(adCanvas);
            DisableBtt();
            Undo.IsEnabled = false;
            Redo.IsEnabled = false;
        }

        // Select button --------------------------------------------------

        private void selected_Button(object sender, System.Windows.RoutedEventArgs e)
        {
            RadioButton rbt = (RadioButton)sender;
            shape.RemoveAdorner(adlayer);
            paintCanvas.Children.Remove(SelectRectangle);
            m_Rect = null;
            RemoveRectAdorner(copyImage);
            RemoveRectAdorner(currentSelectedImage);
            switch (rbt.Name.ToString())
            {
                case "Line":
                    type = DrawType.CLine; break;
                case "Rectangle":
                    type = DrawType.CRectangle; break;
                case "Square":
                    type = DrawType.CSquare; break;
                case "Ellipse":
                    type = DrawType.CEllipse; break;
                case "Circle":
                    type = DrawType.CCircle; break;
                case "Heart":
                    type = DrawType.CHeart; break;
                case "Star":
                    type = DrawType.CStar; break;
                case "Arrow":
                    type = DrawType.CArrow; break;
                case "Select":
                    type = DrawType.CSelect; break;
                case "Text":
                    type = DrawType.CText; break;
                case "Fill":
                    type = DrawType.CFill; break;
            }
            if (type != DrawType.CSelect)
            {
                Select.IsChecked = false;
                DisableBtt();
            }
            else
            {
                EnableButton();
                Text.IsChecked = false;
                Ellipse.IsChecked = false;
                Line.IsChecked = false;
                Rectangle.IsChecked = false;
                Square.IsChecked = false;
                Circle.IsChecked = false;
                Heart.IsChecked = false;
                Arrow.IsChecked = false;
                Star.IsChecked = false;
                Fill.IsChecked = false;

            }
            if (type != DrawType.CText)
            {
                Text.IsChecked = false;
            }
            else
            {
                Select.IsChecked = false;
                Ellipse.IsChecked = false;
                Line.IsChecked = false;
                Rectangle.IsChecked = false;
                Square.IsChecked = false;
                Circle.IsChecked = false;
                Heart.IsChecked = false;
                Arrow.IsChecked = false;
                Star.IsChecked = false;
                Fill.IsChecked = false;
            }

            if (type != DrawType.CFill)
            {
                Fill.IsChecked = false;
            }
            else
            {
                Text.IsChecked = false;
                Select.IsChecked = false;
                Ellipse.IsChecked = false;
                Line.IsChecked = false;
                Rectangle.IsChecked = false;
                Square.IsChecked = false;
                Circle.IsChecked = false;
                Heart.IsChecked = false;
                Arrow.IsChecked = false;
                Star.IsChecked = false;
            }
        }

        private void RemoveRectAdorner(Image Element)
        {
            if (Element != null)
            {
                if (adlayer != null)
                {
                    Adorner[] adorners = adlayer.GetAdorners(Element);
                    if (adorners != null)
                    {
                        foreach (var adorner in adorners)
                            adlayer.Remove(adorner);
                    }
                }
            }
        }

        private void EnableButton()
        {
            Cut.IsEnabled = true;
            Copy.IsEnabled = true;
            Paste.IsEnabled = true;
        }

        private void DisableBtt()
        {
            Cut.IsEnabled = false;
            Copy.IsEnabled = false;
            Paste.IsEnabled = false;
        }

        private void colorPicker_SelectedColorChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (colorBit == true)
            {
                fiveColorLGB = new LinearGradientBrush();
                fiveColorLGB.StartPoint = new Point(0, 0);
                fiveColorLGB.EndPoint = new Point(1, 0);

                GS1 = new GradientStop();
                GS1.Color = (Color)colorPicker2.SelectedColor;
                GS1.Offset = 1- (float)Offset.Value;
                fiveColorLGB.GradientStops.Add(GS1);

                GS2 = new GradientStop();
                GS2.Color = (Color)colorPicker3.SelectedColor;
                GS2.Offset = 1;
                fiveColorLGB.GradientStops.Add(GS2);

                SolidColorBrush color1 = new SolidColorBrush((Color)colorPicker1.SelectedColor);
                shape.ChangeColor(color1, fiveColorLGB);
            }
        }

        private void OffsetSlopeChange(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (colorBit == true)
            {
                fiveColorLGB = new LinearGradientBrush();
                fiveColorLGB.StartPoint = new Point(0, 0);
                fiveColorLGB.EndPoint = new Point(1, 0);

                GS1 = new GradientStop();
                GS1.Color = (Color)colorPicker2.SelectedColor;
                GS1.Offset = 1-(float)Offset.Value;
                fiveColorLGB.GradientStops.Add(GS1);

                GS2 = new GradientStop();
                GS2.Color = (Color)colorPicker3.SelectedColor;
                GS2.Offset = 1;
                fiveColorLGB.GradientStops.Add(GS2);

                SolidColorBrush color1 = new SolidColorBrush((Color)colorPicker1.SelectedColor);
                shape.ChangeColor(color1, fiveColorLGB);
            }
        }

        //Copy - paste -------------------------------------------------------------

        int cop = 0;
        Image copyImage = new Image();
        bool AlreadyPaste = false;
        Adorner imagecopy_adorner;

        private void Cut_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (currentSelectedImage == null) return;
            copyImage = currentSelectedImage;
            listCanvasChildren.Add(currentSelectedImage);
            paintCanvas.Children.Remove(currentSelectedImage);
            AlreadyPaste = false;
        }

        private void Copy_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (currentSelectedImage == null) return;
            copyImage.Source = currentSelectedImage.Source;
            imagecopy_adorner = new SelectAdorner(copyImage);
            adlayer.Add(imagecopy_adorner);
            AlreadyPaste = false;
            cop = 1;
        }

        private void Paste_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (AlreadyPaste == true) return;
            if (currentSelectedImage != null)
            {
                if (adlayer != null)
                {
                    Adorner[] adorners = adlayer.GetAdorners(currentSelectedImage);
                    if (adorners != null)
                    {
                        foreach (var adorner in adorners)
                            adlayer.Remove(adorner);
                    }
                }

                currentSelectedImage = null;             
            }
            if (copyImage == null) return;
            {
                Canvas.SetTop(copyImage, 0);
                Canvas.SetLeft(copyImage, 0);
                paintCanvas.Children.Add(copyImage);
                imagecopy_adorner = new SelectAdorner(copyImage);
                adlayer.Add(imagecopy_adorner);
            }
            RemoveRectAdorner(currentSelectedImage);
            currentSelectedImage = new Image();
            currentSelectedImage.Source = copyImage.Source;
            copyImage = new Image();
            AlreadyPaste = true;
        }

        //Redo - Undo ----------------------------------------------

        List<UIElement> listCanvasChildren = new List<UIElement>();
        List<BitmapImage> listBitmapImage = new List<BitmapImage>();
        int nImage = 0;
        int nvt = 0;
        private void Redo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Undo.IsEnabled = true;
            if (nvt < nImage)
            {      
                oriimage = listBitmapImage[nvt];
                nvt++;
                ImageBrush image = new ImageBrush();
                image.ImageSource = oriimage;
                paintCanvas.Children.Clear();

                if (adCanvas == null || adlayer == null) return;


                paintCanvas.Height = image.ImageSource.Height;
                paintCanvas.Width = image.ImageSource.Width;

                adlayer.Remove(adCanvas);
                adlayer = AdornerLayer.GetAdornerLayer(paintCanvas);
                adCanvas = new CanvasAdorner(paintCanvas);
                adlayer.Add(adCanvas);

                paintCanvas.Background = image;
            }
            if (nvt == nImage)
                Redo.IsEnabled = false;
        }

        private void Undo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Redo.IsEnabled = true;
            if (nvt > 1)
            {
                oriimage = listBitmapImage[nvt - 2];
                nvt--;
                ImageBrush image = new ImageBrush();
                image.ImageSource = oriimage;
                paintCanvas.Children.Clear();

                if (adCanvas == null || adlayer == null) return;


                paintCanvas.Height = image.ImageSource.Height;
                paintCanvas.Width = image.ImageSource.Width;

                adlayer.Remove(adCanvas);
                adlayer = AdornerLayer.GetAdornerLayer(paintCanvas);
                adCanvas = new CanvasAdorner(paintCanvas);
                adlayer.Add(adCanvas);

                paintCanvas.Background = image;
            }
            if (nvt == 1)
                Undo.IsEnabled = false;
        }

        //Fill -------------------------------------------------------
        byte[] pixels;
        byte red1, green1, blue1, alpha1; // mau goc
        byte red2, green2, blue2, alpha2; // mau can to
        int stride = 0;
        int maxW, maxH;
        private void Set_Fill()
        {
            BitmapImage img = oriimage;
            WriteableBitmap wbmap = new WriteableBitmap(img);

            int width = oriimage.PixelWidth;
            int height = oriimage.PixelHeight;
            maxH = height; maxW = width;
            stride = oriimage.PixelWidth * 4;
            int size = oriimage.PixelHeight * stride;
            pixels = new byte[size];
            oriimage.CopyPixels(pixels, stride, 0);

            int x = (int)startPoint.X;
            int y = (int)startPoint.Y;
            int u = (int)startPoint.X;
            int t = (int)startPoint.Y;
            int index = y * stride + 4 * x;

            blue1 = pixels[index];
            green1 = pixels[index + 1];
            red1 = pixels[index + 2];
            alpha1 = pixels[index + 3];

            red2 = colorPicker2.SelectedColor.Value.R;
            green2 = colorPicker2.SelectedColor.Value.G;
            blue2 = colorPicker2.SelectedColor.Value.B;
            alpha2 = colorPicker2.SelectedColor.Value.A;

            Point aaa = new Point();
            aaa.X = x;
            aaa.Y = y;

            FloodFill(aaa);

            Int32Rect rect = new Int32Rect(0, 0, width, height);
            stride = 4 * width;
            wbmap.WritePixels(rect, pixels, stride, 0);

            ImageBrush image = new ImageBrush();
            image.ImageSource = wbmap;
            paintCanvas.Children.Clear();

            if (adCanvas == null || adlayer == null) return;

            paintCanvas.Height = image.ImageSource.Height;
            paintCanvas.Width = image.ImageSource.Width;

            adlayer.Remove(adCanvas);
            adlayer = AdornerLayer.GetAdornerLayer(paintCanvas);
            adCanvas = new CanvasAdorner(paintCanvas);
            adlayer.Add(adCanvas);
            oriimage = wbmap.ToBitmapImage();
            paintCanvas.Background = image;
        }

        private void To_Loang1(int x, int y)
        {
            int index = y * stride + 4 * x;
            if (red1 == red2 && blue1 == blue2 && alpha1 == alpha2 && green1 == green2)
                return;
            if (pixels[index] == blue1 && pixels[index + 1] == green1 && pixels[index + 2] == red1 && pixels[index + 3] == alpha1)
            {
                pixels[index] = blue2;
                pixels[index + 1] = green2;
                pixels[index + 2] = red2;
                pixels[index + 3] = alpha2;
                if (x + 1 < maxW)
                    To_Loang1(x + 1, y);
                if (y + 1 < maxH)
                    To_Loang1(x, y + 1);
                if (y - 1 >= 0)
                    To_Loang1(x, y - 1);
            }
            else
                return;
        }

        private void To_Loang2(int x, int y)
        {
            int index = y * stride + 4 * x;
            if (red1 == red2 && blue1 == blue2 && alpha1 == alpha2 && green1 == green2)
                return;
            if (pixels[index] == blue1 && pixels[index + 1] == green1 && pixels[index + 2] == red1 && pixels[index + 3] == alpha1)
            {
                pixels[index] = blue2;
                pixels[index + 1] = green2;
                pixels[index + 2] = red2;
                pixels[index + 3] = alpha2;
                if (x - 1 >= 0)
                    To_Loang2(x - 1, y);
                if (y + 1 < maxH)
                    To_Loang2(x, y + 1);
                if (y - 1 >= 0)
                    To_Loang2(x, y - 1);
            }
            else
                return;
        }

        private void FloodFill(Point startPoint)
        {
            Queue<Point> queue = new Queue<Point>();
            if (red1 == red2 && blue1 == blue2 && alpha1 == alpha2 && green1 == green2)
                return;
            queue.Enqueue(startPoint);

            while (queue.Count > 0)
            {
                
                Point currentPoint = queue.Dequeue();
                int index = (int)currentPoint.Y * stride + 4 * (int)currentPoint.X;
                //int com = CompareColors(pixels[index], pixels[index + 1], pixels[index + 2], pixels[index + 3], blue1, green1, red1, alpha1);
                if (pixels[index] == blue1 && pixels[index + 1] == green1 && pixels[index + 2] == red1 && pixels[index + 3] == alpha1)
                {
                    pixels[index] = blue2;
                    pixels[index + 1] = green2;
                    pixels[index + 2] = red2;
                    pixels[index + 3] = alpha2;
                    if (currentPoint.X + 1 < maxW)
                        queue.Enqueue(new Point(currentPoint.X + 1, currentPoint.Y));
                    if (currentPoint.X - 1 >= 0)
                        queue.Enqueue(new Point(currentPoint.X - 1, currentPoint.Y));
                    if (currentPoint.Y + 1 < maxH)
                        queue.Enqueue(new Point(currentPoint.X, currentPoint.Y + 1));
                    if (currentPoint.Y - 1 >= 0)
                        queue.Enqueue(new Point(currentPoint.X, currentPoint.Y - 1));
                }

            }
        }

        public static int CompareColors(byte B1, byte G1, byte R1, byte A1, byte B2, byte G2, byte R2, byte A2)
        {
            return 100 * (int)(1.0 - ((double)( Math.Abs(R1 - R2) + Math.Abs (G1 - G2) + Math.Abs(B1 - B2) ) / (255.0 * 3))
            );
        }

        private void borderSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (adlayer != null)
                shape.ChangeThickness(Int32.Parse(borderSizeComboBox.SelectedItem.ToString()));
        }
    }
    public static class ImageHelpers
    {
        public static BitmapImage ToBitmapImage(this WriteableBitmap wbm)
        {
            BitmapImage bmImage = new BitmapImage();
            using (MemoryStream stream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(wbm));
                encoder.Save(stream);
                bmImage.BeginInit();
                bmImage.CacheOption = BitmapCacheOption.OnLoad;
                bmImage.StreamSource = stream;
                bmImage.EndInit();
                bmImage.Freeze();
            }
            return bmImage;
        }
    }

}
   