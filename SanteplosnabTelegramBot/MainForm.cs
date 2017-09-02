using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SanteplosnabTelegramBot
{
    public partial class MainForm : Form
    {
        BackgroundWorker bw;

        public MainForm()
        {
            InitializeComponent();

            this.bw = new BackgroundWorker();

            this.bw.DoWork += this.bw_DoWork;
        }

        /// <summary>
        /// Основной процесс бота
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var key = e.Argument as string;

            try
            {
                var Bot = new Telegram.Bot.TelegramBotClient(key);

                await Bot.SetWebhookAsync("");

                int offset = 0;

                // Основной цикл программы
                while (true)
                {
                    var updates = await Bot.GetUpdatesAsync(offset);

                    foreach(var update in updates)
                    {
                        var message = update.Message;

                        Console.WriteLine(update.Type);

                        if(message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
                        {
                            if (message.Text.Equals("/тест"))
                            {
                                await Bot.SendTextMessageAsync(message.Chat.Id, "Привет, это бот компании СанТеплоСнаб. В данный момент меня еще пилят " +
                                    "и я ничего не умею :(", replyToMessageId: message.MessageId);
                            }

                            if (message.Text.Equals("/счет"))
                            {
                                await Bot.SendTextMessageAsync(message.Chat.Id, "От какой организации выставить счет?",
                                    replyToMessageId: message.MessageId);
                            }
                        }
                        offset += update.Id + 1;
                    }
                }


            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            var text = textBoxApiKey.Text;

            if (!this.bw.IsBusy)
            {
                this.bw.RunWorkerAsync(text);
            }
        }
    }
}
