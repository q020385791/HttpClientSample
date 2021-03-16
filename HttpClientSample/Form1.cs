using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HttpClientSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private  async void Post_Click(object sender, EventArgs e)
        {
            //目標api網址
            string TargetUri = "";
            
            //資料結構包裝
            Model _Model = new Model();

            HttpClient client = new HttpClient();

            #region 資料輸入
            _Model.MProp1 = "This is Prop1";
            _Model.MProp2 = "This is Prop2";
            _Model.Data.ModelProp1 = "This is ModelProp1";
            _Model.Data.ModelProp2 = "This is ModelProp2";
            _Model.lstData.Add(_Model.Data);
            _Model.lstData.Add(_Model.Data);
            _Model.lstData.Add(_Model.Data);

            #endregion
            //資料轉為Json格式文字 再轉為byteArraycontent
            var content = JsonConvert.SerializeObject(_Model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            var request = new HttpRequestMessage
            {
                //設定為Post
                Method = HttpMethod.Post,
                //設定目標網址
                RequestUri = new Uri(TargetUri)
            };
            //資料輸入
            request.Content = new StringContent(content,
                                    Encoding.UTF8,
                                    "application/json");

            #region Header
            //掛上自訂標頭
            request.Headers.Add("CustomHeader1", "^.<");
            request.Headers.Add("CustomHeader2", "=..=");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = await client.SendAsync(request);
            string resultContent = await result.Content.ReadAsStringAsync();
            //回覆一樣可以用Model包裝 Json名稱欄位要對傳回值
            Model Resp = JsonConvert.DeserializeObject<Model>(resultContent);
            #endregion
        }

        public class Model
        {
            public Model() 
            {
                Data = new ModelData();
                lstData = new List<ModelData>();
            }
            public string MProp1 { get; set; }
            public ModelData Data { get; set; }
            public string MProp2 { get; set; }

            public List<ModelData> lstData { get; set; }
        }

        public class ModelData
        {
        public string ModelProp1 { get; set; }
        public string ModelProp2 { get; set; }        
        }

        private async void button1_Click(object sender, EventArgs e)
        {     
            //目標api網址
            string TargetUri = "";
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage
            {
                //設定為Get
                Method = HttpMethod.Get,
                //設定目標網址
                RequestUri = new Uri(TargetUri)
            };
           var result=client.SendAsync(request);
            //解析為Json 與Post一樣可以包裝至object(這裡沒包)
            string resultContent = await result.Result.Content.ReadAsStringAsync();
            

        }
    }
}
