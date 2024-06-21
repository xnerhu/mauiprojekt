using OpenAI_API;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mauiprojekt {
    public class Base64ToImageConverter : IValueConverter {
        public static string TestBase64String =>
            "/9j/4AAQSkZJRgABAQACWAJYAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/wgALCAPAB4ABAREA/8QAHAABAAIDAQEBAAAAAAAAAAAAAAcIAwUGBAIB/9oACAEBAAAAAJ/AAAAAAAAAAAAAAAAAAADFQzAAAAAAAAAAAAAAAAAAAAMt+gAAAAAAAAAAAAAAAAAAAxUMwAAAAAAAAAAAAAAAAAAADLfoAAAAAAAAAAAAAAAAAAAMVDMAAAAAAAAAAAAAAAAAAAAy36AAAAAAAAAAAAAAAAAAADFQzAAAAAAAAAAAAAAAAAAAAMt+gAAAAAAAAAAAAAAAAAAAxUMwAAAAAAAAAAAAAAAAAAADLfoAAAAAAAAAAAAAAAAAAAMVDMAAAAAAAAAAAAAAAAAAAAy36AAAAAAAAAAAAAAAAAAADFQzAAAAAAAAAAAAAAAAAAAAMt+gAAAAAAAAAAAAAAAAAAAxUMwAAAAAAAAAAAAAAAAAAADLfoAAAAAAAAAAAAAAAAAAAMVDMAAAAAAAAAAAAAAAAAAAAy36AAAAAAAAAAAAAAAAAAADFQzAAAAAAAAAAAAAAAAAAAAMt+gAAAAAAAAebRajV+LCz+zZ7fe+oAAAAAAAAMVDMAAAAAAAAAerebfaezOw+LWafR+UAAAAAAAAMt+gAAAAAAAPPxvB8ZyPP+cAejoOt7Tu+z9AAAAAAAAMVDMAAAAAAAAPR2Xedn13QegAefn+R4zg+N84AAAAAAAMt+gAAAAAAB4o3i2PdSAAA2shSlJPtAAAAAAAMVDMAAAAAAAB7ZJlKQtqAAA1MexbG3jAAAAAAAMt+gAAAAAAHIwxFOrAAAAbSVpn64AAAAAADFQzAAAAAAAB18zyrtAAAAGqiqF+SAAAAAAAy36AAAAAABwUERziAAAABkked+9AAAAAADFQzAAAAAAAHfzvI";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null)
                return null;

            var base64String = value.ToString();
            if (string.IsNullOrEmpty(base64String))
                return null;

            var bytes = System.Convert.FromBase64String(base64String);
            return ImageSource.FromStream(() => new MemoryStream(bytes));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class TaskDetailsViewModel {


        public TaskModel Task { get; set; }
        public TaskDatabase TaskDatabase { get; private set; }
        public TaskListViewModel TaskListViewModel;


        /* public ICommand PickImageCommand { get; set; }
         public ICommand RemoveImageCommand { get; set; }
        */


        public TaskDetailsViewModel(TaskModel task, TaskDatabase TaskDatabase, TaskListViewModel TaskListViewModel) {
            this.Task = task;
            this.TaskDatabase = TaskDatabase;
            this.TaskListViewModel = TaskListViewModel;


            /*       PickImageCommand = new Command(async () => await PickImage());
                   RemoveImageCommand = new Command(RemoveImage);*/
        }

        public async Task RefineDescription() {
            Console.WriteLine("Refining task description");
            OpenAIAPI api = new OpenAIAPI(Constants.OPENAI_API_KEY);
            var chat = api.Chat.CreateConversation();
            chat.Model = OpenAI_API.Models.Model.GPT4_Turbo;
            chat.RequestParameters.Temperature = 0.8;

            chat.AppendSystemMessage("Your goal is to refine task description given to you. You must output only refined task description and nothing else.\nTask title: " + Task.Title);
            chat.AppendUserInput(Task.Description);
            string response = await chat.GetResponseFromChatbotAsync();
            Console.WriteLine(response);

            Task.Description = response;
        }


        /* private ImageSource _attachmentImageSource;
         public ImageSource AttachmentImageSource {
             get { return _attachmentImageSource; }
             set {
                 _attachmentImageSource = value;
                 OnPropertyChanged();
                 Console.WriteLine("AttachmentImageSource property changed.");
             }
         }
        */

        /*  public async Task PickImage() {
               var result = await MediaPicker.PickPhotoAsync();
               if (result != null) {
                   var stream = await result.OpenReadAsync();

                   using (MemoryStream ms = new MemoryStream()) {
                       stream.CopyTo(ms);
                       Task.Image = Convert.ToBase64String(ms.ToArray());
                   }

                   byte[] base64Stream = Convert.FromBase64String(Task.Image);
                   Device.BeginInvokeOnMainThread(() => {
                       AttachmentImageSource = ImageSource.FromStream(() => new MemoryStream(base64Stream));
                   });

               }
          }

          public void RemoveImage() {
                Task.Image = null;
                AttachmentImageSource = null;
                OnPropertyChanged(nameof(Task.Image));
                OnPropertyChanged(nameof(AttachmentImageSource));
          }

          public event PropertyChangedEventHandler PropertyChanged;

          protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
              PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
          }*/
    }
}