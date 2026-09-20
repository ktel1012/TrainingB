using System.Text.Json;

namespace TrainingB.Mobile;

public class MainPage : ContentPage
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri(ApiSettings.BaseUrl),
        Timeout = TimeSpan.FromMinutes(15)
    };

    private readonly Label _statusLabel;
    private readonly Editor _resultsEditor;
    private readonly ActivityIndicator _activityIndicator;
    private readonly List<Button> _scraperButtons = [];

    private static readonly (string Name, string Description)[] Scrapers =
    [
        ("HN_2D_XIEN", "HN - 2D Xiên"),
        ("HN_3D_DAU", "HN - 3D Đầu"),
        ("HN_3D_DUOI", "HN - 3D Đuôi"),
        ("HN_3D_LO", "HN - 3D Lô"),
        ("HN_4D_DUOI", "HN - 4D Đuôi"),
        ("HN_4D_LO", "HN - 4D Lô"),
        ("MN_2D_DAU", "MN - 2D Đầu"),
        ("MN_2D_DUOI", "MN - 2D Đuôi"),
        ("MN_3D_DAU", "MN - 3D Đầu"),
        ("MN_3D_DUOI", "MN - 3D Đuôi"),
        ("MN_3D_LO", "MN - 3D Lô"),
        ("MN_4D_DUOI", "MN - 4D Đuôi"),
        ("MN_4D_LO", "MN - 4D Lô")
    ];

    public MainPage()
    {
        Title = "TrainingB";
        BackgroundColor = Colors.White;

        _statusLabel = new Label
        {
            Text = "Sẵn sàng",
            TextColor = Colors.DarkSlateGray,
            FontSize = 15
        };

        _activityIndicator = new ActivityIndicator
        {
            IsVisible = false,
            IsRunning = false,
            Color = Colors.DodgerBlue
        };

        _resultsEditor = new Editor
        {
            Placeholder = "Kết quả sẽ hiển thị ở đây",
            IsReadOnly = true,
            AutoSize = EditorAutoSizeOption.TextChanges,
            MinimumHeightRequest = 180,
            BackgroundColor = Color.FromArgb("#F1F5F9")
        };

        var layout = new VerticalStackLayout
        {
            Padding = new Thickness(16, 12),
            Spacing = 10
        };

        layout.Children.Add(new Label
        {
            Text = "TrainingB Lottery Scraper",
            FontSize = 24,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#1D4ED8")
        });
        layout.Children.Add(new Label
        {
            Text = "Chọn một scraper để chạy trên server",
            FontSize = 14,
            TextColor = Colors.Gray
        });
        layout.Children.Add(_statusLabel);
        layout.Children.Add(_activityIndicator);

        foreach (var scraper in Scrapers)
        {
            var button = new Button
            {
                Text = scraper.Description,
                CommandParameter = scraper.Name,
                BackgroundColor = Color.FromArgb("#2563EB"),
                TextColor = Colors.White,
                CornerRadius = 8,
                HeightRequest = 48
            };
            button.Clicked += ScraperButtonClicked;
            _scraperButtons.Add(button);
            layout.Children.Add(button);
        }

        layout.Children.Add(new Label
        {
            Text = "Kết quả",
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            Margin = new Thickness(0, 10, 0, 0)
        });
        layout.Children.Add(_resultsEditor);

        Content = new ScrollView { Content = layout };
    }

    private async void ScraperButtonClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button || button.CommandParameter is not string scraperName)
            return;

        SetBusy(true, $"Đang chạy {button.Text}... Có thể mất vài phút");

        try
        {
            using var response = await _httpClient.PostAsync($"api/scraper/run/{scraperName}", null);
            var body = await response.Content.ReadAsStringAsync();
            _resultsEditor.Text = FormatResponse(response.IsSuccessStatusCode, body);
            _statusLabel.Text = response.IsSuccessStatusCode
                ? $"Hoàn thành: {button.Text}"
                : $"Lỗi HTTP {(int)response.StatusCode}: {button.Text}";
        }
        catch (TaskCanceledException)
        {
            _statusLabel.Text = "Đã hết thời gian chờ. Hãy kiểm tra Render logs.";
            _resultsEditor.Text = "Request timeout sau 15 phút.";
        }
        catch (Exception ex)
        {
            _statusLabel.Text = "Không kết nối được backend";
            _resultsEditor.Text = ex.Message;
        }
        finally
        {
            SetBusy(false, _statusLabel.Text);
        }
    }

    private void SetBusy(bool busy, string status)
    {
        _statusLabel.Text = status;
        _activityIndicator.IsVisible = busy;
        _activityIndicator.IsRunning = busy;
        foreach (var button in _scraperButtons)
            button.IsEnabled = !busy;
    }

    private static string FormatResponse(bool success, string body)
    {
        try
        {
            using var document = JsonDocument.Parse(body);
            var root = document.RootElement;

            if (root.TryGetProperty("result", out var result))
                return result.GetString() ?? result.ToString();
            if (root.TryGetProperty("error", out var error))
                return $"Lỗi: {error.GetString() ?? error.ToString()}";
        }
        catch (JsonException)
        {
            // Show the raw body below when the backend response is not JSON.
        }

        return success ? body : $"Lỗi backend: {body}";
    }
}