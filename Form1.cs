using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace parser_HH
{
    public partial class Form1 : Form
    {

        private bool _isPageLoaded = false;
        List<string> refers = new List<string>();
        int index = 1;

        public Form1()
        {
            InitializeComponent();
            label1.Text = "";
            label2.Text = "";
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string LastRefer = Properties.Settings.Default.LastRefer;
            if (LastRefer == null || LastRefer == "")
                txtURL.Text = "https://volgograd.hh.ru/search/vacancy?text=%D0%BC%D0%B5%D0%BD%D0%B5%D0%B4%D0%B6%D0%B5%D1%80+%D0%BF%D0%BE+%D0%BF%D1%80%D0%BE%D0%B4%D0%B0%D0%B6%D0%B0%D0%BC&excluded_text=&area=1&salary=200%E2%80%89000&salary=200000&salary_mode=&currency_code=RUR&experience=doesNotMatter&employment_form=FULL&work_schedule_by_days=FIVE_ON_TWO_OFF&work_format=ON_SITE&work_format=REMOTE&work_format=HYBRID&order_by=relevance&search_period=30&items_on_page=100&L_save_area=true&hhtmFrom=vacancy_search_filter";
            else
                txtURL.Text = Properties.Settings.Default.LastRefer;

            int CountRuns = Properties.Settings.Default.CountRuns;
            
            if (CountRuns > 10) 
                Application.Exit();
            else
                Properties.Settings.Default.CountRuns = 1;


            // Опционально: Указываем папку для данных браузера (куки, кеш и т.д.)
            // Если не указать, будет использоваться папка по умолчанию
            CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync();
            await webView.EnsureCoreWebView2Async(environment);

            // Можно сразу загрузить стартовую страницу, если нужно
            webView.Source = new Uri("https://hh.ru");
        }

        private async void btnStartParse_Click(object sender, EventArgs e)
        {
            string checkElementScript = @"
            //(function() {
                var element = document.querySelector('.vacancy-serp-content');
                 element ? true : false;
            //})();";

            string result = await webView.CoreWebView2.ExecuteScriptAsync(checkElementScript);

            // Результат возвращается как JSON-строка, поэтому убираем кавычки
            bool elementExists = bool.Parse(result.Replace("\"", ""));

            if (elementExists)
            {
                label1.BackColor = Color.Navy;
                label1.Text = "Найден блок с вакансиями ..";
                label2.Text = "Считываю данные ..";
                await ParseRefers();
                await ParseVacancies();
                label1.Text = "Парсинг завершен успешно!";
                label2.Text = $"Обработано {refers.Count} вакансий";
            }
            else
            {
                label1.Text = "Вакансии не обнаружены!";
                label1.BackColor = Color.Red;
                label2.Text = "";
                MessageBox.Show("Блок с вакансиями не обнаружен");
            }

            //isReady = false;

        }

        private async void btnOpenLink_Click(object sender, EventArgs e)
        {
            addHistory("Загружаю страницу ..", "Дождитесь загрузки страницы.");
            Properties.Settings.Default.LastRefer = txtURL.Text;
            Properties.Settings.Default.Save();

            webView.Source = new Uri(txtURL.Text);
            
            Task.Delay(10000);
            await WaitForNavigationComplete();
            addHistory("Для начала парсинга нажмите \"Начать сбор данных\"", "");
            MessageBox.Show("Страница загружена. Можно начинать парсинг.");
            
        }

        private void addHistory(string text1, string text2)
        {
            label1.Text = text1; label2.Text = text2;
            label1.BackColor = Color.Navy;
        }


        private async Task ParseRefers()
        {
            int max_pages = 1;
            refers.Clear();

            string scriptPath = Path.Combine(Application.StartupPath, "parseRefers.js");
            string script = File.ReadAllText(scriptPath);

            index = 0;
            for (var page = 1; page <= 1000; page++)
            {
                label2.Text = $"Считываю данные: {page} ({index}) ";
                Debug.Print($"Обрабатываю страницу: {page}");

                // Выполняем скрипт
                string jsonResult = await webView.CoreWebView2.ExecuteScriptAsync(script);
                jsonResult = jsonResult.Replace("\"", "");
                jsonResult = jsonResult.Replace("\\", "");
                jsonResult = jsonResult.Replace("[", "");
                jsonResult = jsonResult.Replace("]", "");

                var currentRefers = jsonResult.Split(',');
                foreach (var refer in currentRefers)
                {
                    index++;
                    refers.Add(refer);
                    Debug.Print(refer);
                    label2.Text = $"Считываю данные: {page} ({index}) ";
                    Application.DoEvents();
                }


                Debug.Print(refers.ToString());

                if (page == max_pages) break;

                webView.CoreWebView2.Navigate(txtURL.Text + $"&page={page + 1}");
                await WaitForNavigationComplete();
            }

            Debug.Print("---wqewrwqrrewr---");

        }

        private async Task ParseVacancies()
        {
            Debug.Print("Begin parseVacansies");
            string scriptPath = Path.Combine(Application.StartupPath, "parseVacancy.js");
            string script = File.ReadAllText(scriptPath);

            var filename = "result.csv";
            File.WriteAllText(filename, "URL;Телефон;Работодатель;Должность;Оплата;Опыт" + Environment.NewLine, Encoding.UTF8);

            var i = 1;
            foreach (var refer in refers)
            {
                addHistory(i++.ToString() + " из " + refers.Count.ToString() + " (" + refer + ")", "");

                webView.CoreWebView2.Navigate(refer);
                await WaitForNavigationComplete();
                

                // Выполняем процесс парсинга Phone
                string phoneNumber = await ExtractPhoneNumber();

                // Выполняем процесс парсинга Data Vacancy
                string jsonResult = await webView.CoreWebView2.ExecuteScriptAsync(script);
                jsonResult = jsonResult.Trim('\"');
                jsonResult = System.Text.RegularExpressions.Regex.Unescape(jsonResult);
                var vacancy = JsonSerializer.Deserialize<VacancyData>(jsonResult);

                var line = $"{EscapeCsvField(refer)};" +
                            $"{EscapeCsvField(phoneNumber)};" +
                            $"{EscapeCsvField(vacancy.Employer)};" +
                            $"{EscapeCsvField(vacancy.Title)};" +
                            $"{EscapeCsvField(vacancy.Salary)};" +
                            $"{EscapeCsvField(vacancy.Experience)};";

                File.AppendAllText(filename, line + Environment.NewLine, Encoding.UTF8);
            }

            if (MessageBox.Show("Сбор данных завершен.", "Завершено", MessageBoxButtons.YesNo) == DialogResult.OK)
                    Process.Start("result.csv");
        }






        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field)) return "";

            field = field.Replace("Выплаты: ", "");

            if (field.Contains(";") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
                return "\"" + field.Replace("\"", "\"\"") + "\"";

            return field;
        }



        private async Task WaitForNavigationComplete()
        {
            var tcs = new TaskCompletionSource<bool>();

            EventHandler<CoreWebView2NavigationCompletedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                webView.CoreWebView2.NavigationCompleted -= handler;
                tcs.SetResult(e.IsSuccess);
            };

            webView.CoreWebView2.NavigationCompleted += handler;
            await tcs.Task;
        }

        private async Task<string> ExtractPhoneNumber()
        {
            try
            {
                // Шаг 1: Ждем появления и кликаем по кнопке показа контактов
                string buttonClicked = await WaitAndClickButton();

                if (buttonClicked == "null")
                {
                    throw new Exception("Не удалось найти или кликнуть по кнопке контактов");
                }

                // Шаг 2: Ждем появления номера телефона
                return await WaitForPhoneNumber();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при извлечении номера: {ex.Message}");
            }
        }

        private async Task<string> WaitAndClickButton()
        {
            string waitAndClickScript = @"
                window.checkButtonContact = null;
                new Promise((resolve, reject) => {
                    const timeout = 15000;
                    const startTime = Date.now();
                    
                    const checkButton = () => {
                        const button = document.querySelector('[data-qa=""show-employer-contacts show-employer-contacts_top-button""]');
                        if (button) {
                            window.checkButtonContact = true
                            button.click();
                            resolve(true);
                        } else if (Date.now() - startTime >= timeout) {
                            reject(new Error('Таймаут ожидания кнопки контактов'));
                        } else {
                            setTimeout(checkButton, 500);
                        }
                    };
                    checkButton();
                });
            ";


                
            string result = await webView.CoreWebView2.ExecuteScriptAsync(waitAndClickScript);
            return await ReadWindowValue("checkButtonContact");
            //return true;
        }

        private async Task<string> WaitForPhoneNumber()
        {
            string phoneScript = @"
    // Создаем глобальную переменную для хранения результата
    window.phoneNumberResult = null;
    
    new Promise((resolve, reject) => {
        const timeout = 15000;
        const startTime = Date.now();
        
        const checkPhone = () => {
            const phoneSpan = document.querySelector('[data-qa=""vacancy-contacts__phone-number""]');
            if (phoneSpan && phoneSpan.innerText.trim() !== '') {
                // Сохраняем результат в глобальную переменную
                window.phoneNumberResult = phoneSpan.innerText;
                resolve(phoneSpan.innerText);
            } else if (Date.now() - startTime >= timeout) {
                reject(new Error('Таймаут ожидания номера телефона'));
            } else {
                setTimeout(checkPhone, 500);
            }
        };
        checkPhone();
    });
";

            // Запускаем скрипт
            await webView.CoreWebView2.ExecuteScriptAsync(phoneScript);

            return await ReadWindowValue("phoneNumberResult");

        }

        
        private async void btnCheck_Click(object sender, EventArgs e)
        {
         
        }

        private async Task<string> ReadWindowValue(string value) {

            string? result = null;
            int attempts = 0;
            int maxAttempts = 30; // 30 попыток * 500 мс = 15 секунд

            while (result == null && attempts < maxAttempts)
            {
                attempts++;
                await Task.Delay(500); // Ждем 500 мс между проверками

                // Проверяем значение глобальной переменной
                string script = "window." + value;
                string jsonResult = await webView.CoreWebView2.ExecuteScriptAsync(script);

                // Если результат не null и не undefined
                if (!string.IsNullOrEmpty(jsonResult) && jsonResult != "null")
                {
                    result = jsonResult.Trim('"');
                    break;
                }
            }

            return result;
        }

    }


    public class VacancyData
    {
        public string Url { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public string Salary { get; set; } // зарплата
        public string Experience { get; set; } // опыт 
        public string Compensation { get; set; } // выплаты
        public string ScheduleRemote { get; set; } // удаленно
        public string Employer { get; set; } // компания
        public string LocationCity { get; set; }
        public string LocationMetro { get; set; }
    }
    /*
    data-qa="vacancy-serp__vacancy-work-experience" **
    data-qa="vacancy-serp__vacancy-compensation-frequency" **
    data-qa="vacancy-serp__vacancy-employer-text"
    data-qa="vacancy-serp__vacancy-address"
    data-qa="address-metro-station-name"
    data-qa="vacancy-label-work-schedule-remote"
    */
    
}
    
