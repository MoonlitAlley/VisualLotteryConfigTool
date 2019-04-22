using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

public struct TimeCollect
{
    public string LoadStartTime;    //刷新开始时间
    public string LoadEndTime;      
    public string StartTime;        //活动时间
    public string EndTiem;
}

public struct CostItem  //消耗道具
{
    public string ItemID;
    public string TiemLine;     //失效日期
}
public struct CostItemCollection
{
    public CostItem[] costItems;
}


public struct LotteryItem  //奖池
{
    public string ManID;
    public string WemanID;

    public string BagLevel;     //福袋级别， 玩家祈福获得的物品的数值设计
    public string NumOrTime;    //数量/时效  玩家拾取福袋获得的物品数值设计

    public string Probability;  //权重概率
    public string Daily_max;    //日常出最大量
    public string BroadCast;    //是否广播
    public string Flags;    //物品级别
    public string Effect;   //物品级别
    public string Show;     //需要比对
    public string IsReset;  //需要比对
}

public struct EnsureRewardPackage
{
    public string times;
    public LotteryItem[] RewardItemList;
}

public struct LotteryCollection
{
    public LotteryItem[] LotteryPool;       //奖池物品的数值设计

    public LotteryItem[] LevelOneBagPack;   //玩家拾取福袋获得的物品数值设计

    public LotteryItem[] LevelTwoBagPack;

    public LotteryItem[] PrayPool;      //玩家祈福获得的物品的数值设计

    public LotteryItem[] TreasureFitment;      //聚宝池抽奖家具物品展示设计


    public LotteryItem[] Reset;     //重置物品

    //保底次数
    public EnsureRewardPackage[] EnsureLowTimes;

    public LotteryItem[] LowTimesOne;  //保底次数
    public LotteryItem[] LowTimesTwo;
}





//小灵通相关数据   第三页签
public struct ExchangeItem
{
    public string ID;
    public string PriceDiamond;
    public string LimitMax;
    public string Name;     //中文名称  用于写入注释
}

public struct PHSLotteryCollection
{
    public ExchangeItem[] ExchangeItems;
    public ExchangeItem[] OneBtnOpen;   //一键打开
    public ExchangeItem QuickPurchase;  //快捷购买
}




namespace VisualLotteryConfigTool
{
    public partial class VisualLotteryConfigTool : Form
    {
        private Dictionary<string, bool> IsOutPutFIleExist = new Dictionary<string, bool>();
        

        //对应xlsx文件的三个页签，若页签不存在，则DataTable的行列为0;
        private DataTable firstDataTable = new DataTable();
        private bool isFirstDataTableExist = false;
        private DataTable secondDataTable = new DataTable();
        private bool isSecondDataTableExist = false;
        private DataTable thirdDataTable = new DataTable();
        private bool isThirdDataTableExit = false;

        //对Activity节点，或者该节点的ID值进行备份，因为同时有多个文件读取
        private string ActivityNodeIDBackUp;
        private bool IsPHSAddNewFlag = false;       //是否有New角标标记
        

        //数据信息集合    使用顺序至关重要
        //bool ，检查是否已导入系统，每次导入后置为true； 每当输入文件更改是，置为false;
        private TimeCollect timeCollection = new TimeCollect();
        private bool IsTimeCollectionLoad = false;
        private CostItemCollection costItemCollection = new CostItemCollection();
        private bool IsCostItemCollectionLoad = false;
        private LotteryCollection LotteryCollection = new LotteryCollection();
        //判断是否已经将这个结构体读取到系统中了
        private bool IsLotteryCollectionLoad = false;
        private PHSLotteryCollection phsLotteryCollection = new PHSLotteryCollection();
        private bool IsPHSLotteryLoad = false;



        public VisualLotteryConfigTool()
        {
            InitializeComponent();
            IsOutPutFIleExist.Add("GeneralConfigText", false);
            IsOutPutFIleExist.Add("LotteryConfigText", false);
            IsOutPutFIleExist.Add("FitmentsConfigText", false);
        }


        private void btnGenerateAll_Click(object sender, EventArgs e)
        {

            this.LogBox.Text = "";
            //判断文件存在
            if (XlsxConfigFileText.Text == "")
            {
                LogBox.Text = "请选择导入的xls文件！";
                return;
            }
            //遍历下面五个文本框，检测用户是否输入文件
            if(!(GeneralConfigText.Text == ""))
            {
                IsOutPutFIleExist["GeneralConfigText"]=true;

            }
            else
            {
                IsOutPutFIleExist["GeneralConfigText"] = false;
            }

            if(!(LotteryConfigText.Text==""))
            {
                IsOutPutFIleExist["LotteryConfigText"]=true;
            }
            else
            {
                IsOutPutFIleExist["LotteryConfigText"] = false;
            }

            if(!(FitmentsConfigText.Text==""))
            {
                if(!IsOutPutFIleExist["LotteryConfigText"])
                {
                    LogBox.Text = "前置文件未输入，无法输出";
                    return;
                }
                IsOutPutFIleExist["FitmentsConfigText"] = true;
            }
            else
            {
                IsOutPutFIleExist["FitmentsConfigText"] = false;
            }

            if(!(MobileConfigText.Text==""))
            {
                IsOutPutFIleExist["MobileConfigText"]=true;
            }
            else
            {
                IsOutPutFIleExist["MobileConfigText"] = false;
            }

            if(!(TimelinessText.Text==""))
            {
                IsOutPutFIleExist["TimelinessText"]=true;
            }
            else
            {
                IsOutPutFIleExist["TimelinessText"] = false;
            }

            //无待处理文件
            if(IsOutPutFIleExist.Count == 0)
            {
                LogBox.Text = "无待处理文件";
                return;
            }

            //加载xls文件
            FileUtility.LoadXlsFile(XlsxConfigFileText.Text);


            //读取文件页签
            firstDataTable = FileUtility.GetDataTable("奖励设计（随周配置提供）");
            if(!(firstDataTable.Rows.Count==0 && firstDataTable.Columns.Count==0))
            {
                isFirstDataTableExist = true;
            }
            secondDataTable = FileUtility.GetDataTable("保底奖励");
            if (!(secondDataTable.Rows.Count == 0 && secondDataTable.Columns.Count == 0))
            {
                isSecondDataTableExist = true;
            }
            thirdDataTable = FileUtility.GetDataTable("小灵通刮刮乐、跨服抽奖“兑换”功能扩展");
            if (!(thirdDataTable.Rows.Count == 0 && thirdDataTable.Columns.Count == 0))
            {
                isThirdDataTableExit = true;
            }



            ////开始读取数据
            //GetData();

            //OutPutData();


            //xls文件 奖励设计（随周配置提供）页签存在 才会处理 该文件
            if (IsOutPutFIleExist["GeneralConfigText"]&& isFirstDataTableExist)
            {
                OutPutGeneral_lottery_configFile();
            }


            //xls文件，存在某一个页签，就需要处理该文件
            if (IsOutPutFIleExist["LotteryConfigText"] && (isFirstDataTableExist || isSecondDataTableExist || isThirdDataTableExit))
            {
                OutPutVisible_lottery_configFile();
            }


            if (IsOutPutFIleExist["FitmentsConfigText"] && isFirstDataTableExist)
            {
                OutPutVisible_lottery_fitments_configFile();
            }

            if (IsOutPutFIleExist["MobileConfigText"] && isFirstDataTableExist)
            {
                OutPutMobile_configFile();
            }

            if (IsOutPutFIleExist["TimelinessText"] && isFirstDataTableExist)
            {
                OutPutItem_timeliness_configFile();
            }

            this.LogBox.Text = "执行完成";
        }

        //在这个地方，初始化了 costItemCollection
        private void OutPutGeneral_lottery_configFile()
        {
            GetCostItemCollection();
            //待处理的数据长度为0
            if(costItemCollection.costItems==null)
            {
                return;
            }
            //开始输出

            XmlDocument General_lottery_config = new XmlDocument();
            General_lottery_config.Load(GeneralConfigText.Text);

            XmlNode CostItemList = General_lottery_config.SelectSingleNode("GeneralLotteryCost").LastChild;

            XmlNodeList CostItems = General_lottery_config.SelectNodes("/GeneralLotteryCost/CostItemList/CostItem");

            //使用移除所有 , 该功能会删除节点内注释
            //CostItemList.RemoveAll();

            foreach (XmlElement var in CostItems)
            {
                CostItemList.RemoveChild(var);
            }

            for (int i = 0; i < costItemCollection.costItems.Length; i++)
            {
                XmlElement item = General_lottery_config.CreateElement("CostItem");
                item.SetAttribute("item_id", costItemCollection.costItems[i].ItemID);
                item.SetAttribute("priority", (i+1).ToString());
                CostItemList.AppendChild(item);
            }

            //写入到exe位置的临时文件中
            General_lottery_config.Save("general_lottery_config.xml");
            StreamReader streamReader = new StreamReader("general_lottery_config.xml", Encoding.Default);
            string fileString = streamReader.ReadToEnd();
            streamReader.Close();
            //去空格
            fileString = fileString.Replace(" />", "/>");
            this.LogBox.Text = "执行完成general_lottery_config.xml，请选择输出目标...";
            string fileName = "general_lottery_config";
            SaveToOther(fileName, fileString);
            //完成
            this.LogBox.Text = "";
        }

        //在这个地方初始化了 CostItemCollection、TimeCollection、LotteryCollection、PHSLotteryCollection
        private void OutPutVisible_lottery_configFile()
        {
            GetCostItemCollection();
            GetTimeCollection();
            GetLotteryCollection();
            GetPHSLotteryCollection();


            //开始输出

            XmlDocument Visible_lottery_config = new XmlDocument();
            Visible_lottery_config.Load(LotteryConfigText.Text);

            //玩家祈福获得的物品的设计
            if (LotteryCollection.PrayPool != null)
            {
                XmlNode RewardItemList = Visible_lottery_config.SelectSingleNode("/VisibleLottery/PrayReward/RewardItems");
                XmlNodeList RewardItem = Visible_lottery_config.SelectNodes("/VisibleLottery/PrayReward/RewardItems/RewardItem");

                //删除原祈福池内的物品
                foreach (XmlElement var in RewardItem)
                {
                    RewardItemList.RemoveChild(var);
                }
                for (int i = 0; i < LotteryCollection.PrayPool.Length; i++)
                {
                    XmlElement item = Visible_lottery_config.CreateElement("RewardItem");
                    item.SetAttribute("type", "3");
                    item.SetAttribute("para1", LotteryCollection.PrayPool[i].ManID);
                    item.SetAttribute("para2", LotteryCollection.PrayPool[i].WemanID);
                    item.SetAttribute("para3", LotteryCollection.PrayPool[i].BagLevel);
                    item.SetAttribute("probability", LotteryCollection.PrayPool[i].Probability);
                    item.SetAttribute("daily_max", LotteryCollection.PrayPool[i].Daily_max);

                    RewardItemList.AppendChild(item);
                }
            }

            //玩家拾取福袋获得的物品数值设计
            XmlNodeList BagsReward = Visible_lottery_config.SelectNodes("/VisibleLottery/BagsReward/Bag");
            if (LotteryCollection.LevelOneBagPack != null)
            {
                //一级福袋
                XmlNode LevelOneBag = BagsReward[0].FirstChild;
                XmlNodeList RewardItems = LevelOneBag.SelectNodes("RewardItem");

                foreach (XmlElement var in RewardItems)
                {
                    LevelOneBag.RemoveChild(var);
                }

                for (int i = 0; i < LotteryCollection.LevelOneBagPack.Length; i++)
                {
                    XmlElement item = Visible_lottery_config.CreateElement("RewardItem");
                    item.SetAttribute("type", "3");
                    item.SetAttribute("para1", LotteryCollection.LevelOneBagPack[i].ManID);
                    item.SetAttribute("para2", LotteryCollection.LevelOneBagPack[i].WemanID);
                    item.SetAttribute("para3", LotteryCollection.LevelOneBagPack[i].NumOrTime);
                    item.SetAttribute("probability", LotteryCollection.LevelOneBagPack[i].Probability);
                    item.SetAttribute("daily_max", "0");

                    LevelOneBag.AppendChild(item);
                }
            }
            if(LotteryCollection.LevelTwoBagPack != null)
            {
                //二级福袋
                XmlNode LevelTwoBag = BagsReward[1].FirstChild;
                XmlNodeList RewardItems = LevelTwoBag.SelectNodes("RewardItem");

                foreach (XmlElement var in RewardItems)
                {
                    LevelTwoBag.RemoveChild(var);
                }

                for (int i = 0; i < LotteryCollection.LevelTwoBagPack.Length; i++)
                {
                    XmlElement item = Visible_lottery_config.CreateElement("RewardItem");
                    item.SetAttribute("type", "3");
                    item.SetAttribute("para1", LotteryCollection.LevelTwoBagPack[i].ManID);
                    item.SetAttribute("para2", LotteryCollection.LevelTwoBagPack[i].WemanID);
                    item.SetAttribute("para3", LotteryCollection.LevelTwoBagPack[i].NumOrTime);
                    item.SetAttribute("probability", LotteryCollection.LevelTwoBagPack[i].Probability);
                    item.SetAttribute("daily_max", "0");

                    LevelTwoBag.AppendChild(item);
                }
            }

            //修改activity的相关信息
            if(timeCollection.LoadStartTime!="")
            {
                //修改Activity相关信息 id， 时间
                XmlNode activityNode = Visible_lottery_config.SelectSingleNode("/VisibleLottery/Activity");
                string id = ((XmlElement)activityNode).GetAttribute("id");
                ((XmlElement)activityNode).SetAttribute("id", ((int.Parse(id) + 1).ToString()));
                ((XmlElement)activityNode).SetAttribute("activity_begin", timeCollection.LoadStartTime);
                ((XmlElement)activityNode).SetAttribute("lottery_begin", timeCollection.StartTime);
                ((XmlElement)activityNode).SetAttribute("lottery_end", timeCollection.EndTiem);
                ((XmlElement)activityNode).SetAttribute("activity_end", timeCollection.LoadEndTime);

                //到底是配置之前，还是配置之后呀？？？
                //备份Activity的节点ID信息
                ActivityNodeIDBackUp = (int.Parse(id) + 1).ToString();
            }

            //小灵通刮刮乐、跨服抽奖“兑换”功能扩展
            if (phsLotteryCollection.ExchangeItems != null)
            {
                XmlNode ExchangeConfig = Visible_lottery_config.SelectSingleNode("/VisibleLottery/Activity/ExchangeConfig");

                ExchangeConfig.RemoveAll();
                for(int i = 0; i< phsLotteryCollection.ExchangeItems.Length;i++)
                {
                    XmlElement item = Visible_lottery_config.CreateElement("ExchangeItem");

                    item.SetAttribute("item_id", phsLotteryCollection.ExchangeItems[i].ID);
                    item.SetAttribute("price_diamond", phsLotteryCollection.ExchangeItems[i].PriceDiamond);
                    item.SetAttribute("limit_max", phsLotteryCollection.ExchangeItems[i].LimitMax);
                    item.SetAttribute("unit", "个");

                    ExchangeConfig.AppendChild(item);

                    //注释名称
                    XmlComment comment = Visible_lottery_config.CreateComment(phsLotteryCollection.ExchangeItems[i].Name);
                    ExchangeConfig.InsertAfter(comment, item);
                }

                XmlNode OpenPackages = Visible_lottery_config.SelectSingleNode("/VisibleLottery/Activity/OpenPackages");

                OpenPackages.RemoveAll();
                for(int i = 0; i<phsLotteryCollection.OneBtnOpen.Length;i++)
                {
                    XmlElement pkg = Visible_lottery_config.CreateElement("pkg");

                    pkg.SetAttribute("id", phsLotteryCollection.OneBtnOpen[i].ID);

                    OpenPackages.AppendChild(pkg);
                }


                XmlNode QuickBuyPackages = Visible_lottery_config.SelectSingleNode("/VisibleLottery/Activity/QuickBuyPackages");
                ((XmlElement)QuickBuyPackages).SetAttribute("pkg_id", phsLotteryCollection.QuickPurchase.ID);

            }

            //奖池物品的数值设计
            if (LotteryCollection.LotteryPool != null)
            {
                XmlNode RewardItems = Visible_lottery_config.SelectSingleNode("/VisibleLottery/Activity/LotteryReward/RewardItems");

                RewardItems.RemoveAll();

                for(int i = 0; i<LotteryCollection.LotteryPool.Length;i++)
                {
                    XmlElement item = Visible_lottery_config.CreateElement("RewardItem");

                    item.SetAttribute("type", "3");
                    item.SetAttribute("para1", LotteryCollection.LotteryPool[i].ManID);
                    item.SetAttribute("para2", LotteryCollection.LotteryPool[i].WemanID);
                    item.SetAttribute("para3", LotteryCollection.LotteryPool[i].NumOrTime);
                    item.SetAttribute("probability", LotteryCollection.LotteryPool[i].Probability);
                    item.SetAttribute("daily_max", LotteryCollection.LotteryPool[i].Daily_max);
                    item.SetAttribute("broadcast", LotteryCollection.LotteryPool[i].BroadCast);
                    item.SetAttribute("flags", LotteryCollection.LotteryPool[i].Flags);
                    item.SetAttribute("effect", LotteryCollection.LotteryPool[i].Effect);
                    item.SetAttribute("show", LotteryCollection.LotteryPool[i].Show);
                    if(LotteryCollection.LotteryPool[i].IsReset!="")
                    {
                        item.SetAttribute("is_reset", LotteryCollection.LotteryPool[i].IsReset);
                    }

                    RewardItems.AppendChild(item);
                }
            }

            //保底次数
            if(LotteryCollection.EnsureLowTimes != null)
            {
                XmlNode EnsureRewards = Visible_lottery_config.SelectSingleNode("/VisibleLottery/Activity/EnsureRewards");
                EnsureRewards.RemoveAll();
                for (int LowTimes = 0; LowTimes<LotteryCollection.EnsureLowTimes.Length;LowTimes++)
                {
                    XmlElement EnsureReward = Visible_lottery_config.CreateElement("EnsureReward");
                    EnsureReward.SetAttribute("times", LotteryCollection.EnsureLowTimes[LowTimes].times);
                    EnsureRewards.AppendChild(EnsureReward);

                    XmlElement RewardItems = Visible_lottery_config.CreateElement("RewardItems");
                    EnsureReward.AppendChild(RewardItems);

                    for(int i = 0;i<LotteryCollection.EnsureLowTimes[LowTimes].RewardItemList.Length;i++)
                    {
                        XmlElement item = Visible_lottery_config.CreateElement("RewardItem");
                        item.SetAttribute("type", "3");
                        item.SetAttribute("para1", LotteryCollection.EnsureLowTimes[LowTimes].RewardItemList[i].ManID);
                        item.SetAttribute("para2", LotteryCollection.EnsureLowTimes[LowTimes].RewardItemList[i].WemanID);
                        item.SetAttribute("para3", LotteryCollection.EnsureLowTimes[LowTimes].RewardItemList[i].NumOrTime);
                        item.SetAttribute("probability", LotteryCollection.EnsureLowTimes[LowTimes].RewardItemList[i].Probability);

                        RewardItems.AppendChild(item);
                    }
                }
            }

            //XmlNodeList EnsureRewards = Visible_lottery_config.SelectNodes("/VisibleLottery/Activity/EnsureRewards/EnsureReward");
            //if (LotteryCollection.LowTimesOne.Length!=0)
            //{
            //    XmlNode RewardItems = EnsureRewards[0].FirstChild;
            //    RewardItems.RemoveAll();

            //    for(int i = 0;i < LotteryCollection.LowTimesOne.Length; i++)
            //    {
            //        XmlElement item = Visible_lottery_config.CreateElement("RewardItem");

            //        item.SetAttribute("type", "3");
            //        item.SetAttribute("para1", LotteryCollection.LowTimesOne[i].ManID);
            //        item.SetAttribute("para2", LotteryCollection.LowTimesOne[i].WemanID);
            //        item.SetAttribute("para3", LotteryCollection.LowTimesOne[i].NumOrTime);
            //        item.SetAttribute("probability", LotteryCollection.LowTimesOne[i].Probability);

            //        RewardItems.AppendChild(item);
            //    }
            //}
            //if (LotteryCollection.LowTimesTwo.Length != 0)
            //{
            //    XmlNode RewardItems = EnsureRewards[1].FirstChild;
            //    RewardItems.RemoveAll();

            //    for (int i = 0; i < LotteryCollection.LowTimesTwo.Length; i++)
            //    {
            //        XmlElement item = Visible_lottery_config.CreateElement("RewardItem");

            //        item.SetAttribute("type", "3");
            //        item.SetAttribute("para1", LotteryCollection.LowTimesTwo[i].ManID);
            //        item.SetAttribute("para2", LotteryCollection.LowTimesTwo[i].WemanID);
            //        item.SetAttribute("para3", LotteryCollection.LowTimesTwo[i].NumOrTime);
            //        item.SetAttribute("probability", LotteryCollection.LowTimesTwo[i].Probability);

            //        RewardItems.AppendChild(item);
            //    }
            //}


            //使用移除所有 , 该功能会删除节点内注释
            //CostItemList.RemoveAll();


            //写入到exe位置的临时文件中
            Visible_lottery_config.Save("visible_lottery_config.xml");
            StreamReader streamReader = new StreamReader("visible_lottery_config.xml", Encoding.Default);
            string fileString = streamReader.ReadToEnd();
            streamReader.Close();
            //去空格
            fileString = fileString.Replace(" />", "/>");
            this.LogBox.Text = "执行完成visible_lottery_config.xml，请选择输出目标...";
            string fileName = "visible_lottery_config";
            SaveToOther(fileName, fileString);
            //完成
            this.LogBox.Text = "";
        }
        
        private void OutPutMobile_configFile()
        {
            GetTimeCollection();

            XmlDocument Mobile_config = new XmlDocument();
            Mobile_config.Load(MobileConfigText.Text);

            XmlNodeList Footers = Mobile_config.SelectNodes("/MobileConfig/ModuleFooters/NewFooters/Footer");
            //修改id = 6 的选型时间

            foreach(XmlElement Footer in Footers)
            {
                if(Footer.GetAttribute("module_id") == "6")
                {
                    Footer.SetAttribute("start_time", timeCollection.LoadStartTime);
                    Footer.SetAttribute("end_time", timeCollection.EndTiem);
                }
            }

            //写入到exe位置的临时文件中
            Mobile_config.Save("mobile_config.xml");
            StreamReader streamReader = new StreamReader("mobile_config.xml", Encoding.Default);
            string fileString = streamReader.ReadToEnd();
            streamReader.Close();
            //去空格
            fileString = fileString.Replace(" />", "/>");
            this.LogBox.Text = "执行完成mobile_config.xml，请选择输出目标...";
            string fileName = "mobile_config";
            SaveToOther(fileName, fileString);
            //完成
            this.LogBox.Text = "";
        }

        private void OutPutVisible_lottery_fitments_configFile()
        {
            XmlDocument Fitment_Config = new XmlDocument();
            Fitment_Config.Load(FitmentsConfigText.Text);

            XmlNode activity = Fitment_Config.SelectSingleNode("/VisibleLotteryFitment/Activity");

            if(ActivityNodeIDBackUp!= "")
            {
                ((XmlElement)activity).SetAttribute("id", ActivityNodeIDBackUp);
            }

            //写入到exe位置的临时文件中
            Fitment_Config.Save("visible_lottery_fitment_config.xml");
            StreamReader streamReader = new StreamReader("visible_lottery_fitment_config.xml", Encoding.Default);
            string fileString = streamReader.ReadToEnd();
            streamReader.Close();
            //去空格
            fileString = fileString.Replace(" />", "/>");
            this.LogBox.Text = "执行完成visible_lottery_fitment_config.xml，请选择输出目标...";
            string fileName = "visible_lottery_fitment_config";
            SaveToOther(fileName, fileString);
            //完成
            this.LogBox.Text = "";
        }



        private void OutPutItem_timeliness_configFile()
        {
            GetCostItemCollection();

            if(costItemCollection.costItems.Length!=0)
            {
                XmlDocument TimeLinessConfig = new XmlDocument();
                TimeLinessConfig.Load(TimelinessText.Text);

                XmlNodeList infoList = TimeLinessConfig.SelectNodes("/ItemTimeLinessConfig/ItemInfos/info");

                foreach (XmlElement info in infoList)
                {
                    for(int i =0;i<costItemCollection.costItems.Length; i++)
                    {
                        if(costItemCollection.costItems[i].TiemLine != "")
                        {
                            if (info.GetAttribute("type_id") == costItemCollection.costItems[i].ItemID)
                            {
                                info.SetAttribute("duability", costItemCollection.costItems[i].TiemLine);
                            }
                        }  
                    }
                }

                //写入到exe位置的临时文件中
                TimeLinessConfig.Save("item_timeliness_config.xml");
                StreamReader streamReader = new StreamReader("item_timeliness_config.xml", Encoding.Default);
                string fileString = streamReader.ReadToEnd();
                streamReader.Close();
                //去空格
                fileString = fileString.Replace(" />", "/>");
                this.LogBox.Text = "执行完成item_timeliness_config.xml，请选择输出目标...";
                string fileName = "item_timeliness_config";
                SaveToOther(fileName, fileString);
                //完成
                this.LogBox.Text = "";
            }
        }

        //取出消耗道具
        private void GetCostItemCollection()
        {
            //数据已经读入系统
            if(IsCostItemCollectionLoad)
            {
                //已经初始化了
                return;
            }
            //开始读取
            //确认第一个页签存在
            if(!isFirstDataTableExist)
            {
                costItemCollection.costItems = null;
                MessageBox.Show("读取“道具消耗优先级”需要相应页签存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //读取DataTable数据
            Point point_ID = new Point(0,0);
            point_ID = FileUtility.SearchColumn(ref firstDataTable, point_ID, "道具");
            //向下移动两行
            point_ID.Y = point_ID.Y + 2;
            point_ID = FileUtility.SearchRow(ref firstDataTable, point_ID, "道具id");

            int count = FileUtility.CountKey(ref firstDataTable, point_ID);
            CostItem[] costItems = new CostItem[count];
            for (int i = 0; i<count;i++)
            {
                costItems[i].ItemID = firstDataTable.Rows[point_ID.Y + i + 1][point_ID.X].ToString();
                //处理日期，日期中包括时间
                string tempData = firstDataTable.Rows[point_ID.Y + i + 1][point_ID.X+1].ToString();
                if(tempData!="")
                {
                    costItems[i].TiemLine = GetDataAndTime(tempData)[0];
                }
                else
                {
                    costItems[i].TiemLine = "";
                }
            }
            costItemCollection.costItems = costItems;
            IsCostItemCollectionLoad = true;
        }


        //取出    活动时间的设计
        private void GetTimeCollection()
        {
            if (IsTimeCollectionLoad)
            {
                //数据已经读入系统
                return;
            }
            //开始读取
            //确认第一个页签存在
            if (!isFirstDataTableExist)
            {
                timeCollection.LoadStartTime = "";
                MessageBox.Show("读取“活动时间的设计”需要相应页签存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string tempStartData;
            string tempStartTime;
            string[] tempTime;

            //读取DataTable数据
            Point point_LoadStartTime = new Point(0, 0);
            point_LoadStartTime = FileUtility.SearchColumn(ref firstDataTable, point_LoadStartTime, "活动时间的设计");
            //移动一列

            tempStartData = firstDataTable.Rows[point_LoadStartTime.Y + 0][point_LoadStartTime.X + 2].ToString();
            tempStartTime = firstDataTable.Rows[point_LoadStartTime.Y + 0][point_LoadStartTime.X + 3].ToString();
            tempTime = GetDataAndTime(tempStartData, tempStartTime);
            timeCollection.LoadStartTime = tempTime[0] + " " + tempTime[1];

            tempStartData = firstDataTable.Rows[point_LoadStartTime.Y + 1][point_LoadStartTime.X + 2].ToString();
            tempStartTime = firstDataTable.Rows[point_LoadStartTime.Y + 1][point_LoadStartTime.X + 3].ToString();
            tempTime = GetDataAndTime(tempStartData, tempStartTime);
            timeCollection.StartTime = tempTime[0] + " " + tempTime[1];

            tempStartData = firstDataTable.Rows[point_LoadStartTime.Y + 2][point_LoadStartTime.X + 2].ToString();
            tempStartTime = firstDataTable.Rows[point_LoadStartTime.Y + 2][point_LoadStartTime.X + 3].ToString();
            tempTime = GetDataAndTime(tempStartData, tempStartTime);
            timeCollection.EndTiem = tempTime[0] + " " + tempTime[1];

            tempStartData = firstDataTable.Rows[point_LoadStartTime.Y + 3][point_LoadStartTime.X + 2].ToString();
            tempStartTime = firstDataTable.Rows[point_LoadStartTime.Y + 3][point_LoadStartTime.X + 3].ToString();
            tempTime = GetDataAndTime(tempStartData, tempStartTime);
            timeCollection.LoadEndTime = tempTime[0] + " " + tempTime[1];

            IsTimeCollectionLoad = true;
        }


        private void GetLotteryCollection()
        {
            if(IsLotteryCollectionLoad)
            {
                //数据已经载入系统
                return;
            }
            //先读取第二个页签，因为其中有第一个页签的依赖项
            if (isSecondDataTableExist)
            {
                //读取第二个页签中的相关数据
                Point point_ID = new Point(0, 0);
                point_ID = FileUtility.SearchColumn(ref secondDataTable, point_ID, "重置物品");
                //此处位置特殊，因为处于最顶行
                point_ID.Y = point_ID.Y + 0;
                point_ID = FileUtility.SearchRow(ref secondDataTable, point_ID, "物品男ID");

                LotteryCollection.Reset = GetLotteryFromData(ref secondDataTable, point_ID, 5);

                point_ID.X = point_ID.Y = 0;
                point_ID = FileUtility.SearchColumn(ref secondDataTable, point_ID, "保底次数");


                int count = FileUtility.CountKeyGlobal(ref secondDataTable, point_ID, "次数");
                LotteryCollection.EnsureLowTimes = new EnsureRewardPackage[count];

                //第一个次数的位置
                Point point_times = point_ID;

                for(int i = 0;i<count;i++)
                {
                    point_times = FileUtility.SearchColumnNext(ref secondDataTable,point_times, "次数");
                    LotteryCollection.EnsureLowTimes[i].times = secondDataTable.Rows[point_times.Y][point_times.X + 1].ToString();
                    point_ID.X = point_times.X + 1;
                    point_ID.Y = point_times.Y + 2;
                    LotteryCollection.EnsureLowTimes[i].RewardItemList = GetLotteryFromData(ref secondDataTable, point_ID, 4);
                }

                //point_ID.Y = point_ID.Y + 3;
                //point_ID = FileUtility.SearchRow(ref secondDataTable, point_ID, "男ID");

                //LotteryCollection.LowTimesOne = GetLotteryFromData(ref secondDataTable, point_ID, 4);

                //point_ID = FileUtility.SearchColumnNext(ref secondDataTable, point_ID, "男ID");
                //LotteryCollection.LowTimesTwo = GetLotteryFromData(ref secondDataTable, point_ID, 4);

            }
            else
            {
                LotteryCollection.Reset = null;
                LotteryCollection.EnsureLowTimes = null;
                //LotteryCollection.LowTimesOne = null;
                //LotteryCollection.LowTimesTwo = null;
                MessageBox.Show("读取重置物品需要相应页签存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //用空值去初始化
            }

            if (isFirstDataTableExist)
            {
                //读取第一个页签中的相关数据
                Point point_ID = new Point(0, 0);
                point_ID = FileUtility.SearchColumn(ref firstDataTable, point_ID, "聚宝池抽奖家具物品展示设计");
                point_ID.Y = point_ID.Y + 1;
                point_ID = FileUtility.SearchRow(ref firstDataTable, point_ID, "男ID");

                LotteryCollection.TreasureFitment = GetLotteryFromData(ref firstDataTable, point_ID, 5);

                point_ID.X = point_ID.Y = 0;

                point_ID = FileUtility.SearchColumn(ref firstDataTable, point_ID, "玩家祈福获得的物品的数值设计");
                point_ID.Y = point_ID.Y + 1;
                point_ID = FileUtility.SearchRow(ref firstDataTable, point_ID, "男ID");

                LotteryCollection.PrayPool = GetLotteryFromData(ref firstDataTable, point_ID, 1);

                point_ID.X = point_ID.Y = 0;

                point_ID = FileUtility.SearchColumn(ref firstDataTable, point_ID, "玩家拾取福袋获得的物品数值设计");
                point_ID.Y = point_ID.Y + 1;
                point_ID = FileUtility.SearchRow(ref firstDataTable, point_ID, "男ID");

                LotteryCollection.LevelOneBagPack = GetLotteryFromData(ref firstDataTable, point_ID, 2);

                point_ID = FileUtility.SearchColumnNext(ref firstDataTable, point_ID, "男ID");
                LotteryCollection.LevelTwoBagPack = GetLotteryFromData(ref firstDataTable, point_ID, 2);


                point_ID.X = point_ID.Y = 0;

                point_ID = FileUtility.SearchColumn(ref firstDataTable, point_ID, "奖池物品的数值设计");
                point_ID.Y = point_ID.Y + 1;
                point_ID = FileUtility.SearchRow(ref firstDataTable, point_ID, "男ID");

                LotteryCollection.LotteryPool = GetLotteryFromData(ref firstDataTable, point_ID, 3);
            }
            else
            {
                LotteryCollection.TreasureFitment = null;
                LotteryCollection.PrayPool = null;
                LotteryCollection.LevelOneBagPack = null;
                LotteryCollection.LevelTwoBagPack = null;
                LotteryCollection.LotteryPool = null;
                //用空值去初始化 ， 防止使用上一次读取的旧数值
                MessageBox.Show("读取奖池物品的数值设计需要相应页签存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            IsLotteryCollectionLoad = true;
        }

        //取出奖池物品、祈福物品、祈福礼袋 ...
        //type = 1 表示 玩家祈福获得的物品的数值设计
        //type = 2 表示 玩家拾取福袋获得的物品数值设计
        //type = 3 表示 奖池物品的数值设计
        //type = 4 表示 保底
        //type = 5， 仅读取ID部分
        private LotteryItem[] GetLotteryFromData(ref DataTable dt , Point ManPoint , int type)
        {
            int count = FileUtility.CountKey(ref dt, ManPoint);
            LotteryItem[] lotteryItems = new LotteryItem[count];

            int probabilitySum = 0;
            //先处理权重倍率   
            //权重倍率分为10倍 、100倍 ， 按照小数点后的位数进行选择，若小数点后位数>2 ,则四四舍五入       
            int ProbaTimes = 1;
            if(type == 1 || type == 2 || type == 3)
            {
                string proba = "";
                int afterDotCount = 0;
                for (int i = 0; i < count; i++)
                {
                    if (type == 1 || type == 3)
                    {
                        proba = dt.Rows[ManPoint.Y + i + 1][ManPoint.X + 4].ToString();
                    }
                    if (type == 2)
                    {
                        proba = dt.Rows[ManPoint.Y + i + 1][ManPoint.X + 5].ToString();
                    }
                    if (proba.Contains("."))
                    {
                        string[] dot = proba.Split('.');
                        if (afterDotCount < dot[1].Length)
                        {
                            afterDotCount = dot[1].Length;
                        }
                    }
                }
                if (afterDotCount>0)
                {
                    if(afterDotCount>1)
                    {
                        ProbaTimes = 100;
                    }
                    else
                    {
                        ProbaTimes = 10;
                    }
                }
            }

           
            for (int i = 0; i<count; i++)
            {
                //读取基础: 男ID ， 女ID
                lotteryItems[i].ManID = dt.Rows[ManPoint.Y+i+1][ManPoint.X].ToString();
                lotteryItems[i].WemanID = dt.Rows[ManPoint.Y+i+1][ManPoint.X+2].ToString();

                if(type == 1)
                {
                    lotteryItems[i].BagLevel = dt.Rows[ManPoint.Y + i + 1][ManPoint.X+3].ToString();
                    lotteryItems[i].Probability = dt.Rows[ManPoint.Y + i + 1][ManPoint.X + 4].ToString();
                    lotteryItems[i].Daily_max = dt.Rows[ManPoint.Y + i + 1][ManPoint.X + 8].ToString();
                    if (lotteryItems[i].Daily_max == "")
                    {
                        lotteryItems[i].Daily_max = "0";
                    }
                }
                if(type == 2)
                {
                    lotteryItems[i].NumOrTime = dt.Rows[ManPoint.Y + i + 1][ManPoint.X + 4].ToString();
                    lotteryItems[i].Probability = dt.Rows[ManPoint.Y + i + 1][ManPoint.X + 5].ToString();
                }
                if(type ==3)
                {
                    //此处需要参考其他信息
                    lotteryItems[i].NumOrTime = dt.Rows[ManPoint.Y + i + 1][ManPoint.X + 3].ToString();
                    if (lotteryItems[i].NumOrTime == "永久")
                    {
                        lotteryItems[i].NumOrTime = "0";
                    }
                    else
                    {
                        lotteryItems[i].NumOrTime = Regex.Replace(lotteryItems[i].NumOrTime, @"[^0-9]+", "");
                    }

                    lotteryItems[i].Probability = dt.Rows[ManPoint.Y + i + 1][ManPoint.X + 4].ToString();

                    lotteryItems[i].Daily_max = dt.Rows[ManPoint.Y + i + 1][ManPoint.X + 6].ToString();
                    if(lotteryItems[i].Daily_max=="")
                    {
                        lotteryItems[i].Daily_max = "0";
                    }

                    lotteryItems[i].BroadCast = dt.Rows[ManPoint.Y + i + 1][ManPoint.X + 7].ToString();

                    lotteryItems[i].Flags = dt.Rows[ManPoint.Y + i + 1][ManPoint.X + 8].ToString();
                    if (lotteryItems[i].Flags == "极品")
                    {
                        lotteryItems[i].Flags = "4";
                        lotteryItems[i].Effect = "1";
                    }
                    else if (lotteryItems[i].Flags == "珍品")
                    {
                        lotteryItems[i].Flags = "2";
                        lotteryItems[i].Effect = "1";
                    }
                    else if(lotteryItems[i].Flags == "精品")
                    {
                        lotteryItems[i].Flags = "1";
                        lotteryItems[i].Effect = "1";
                    }
                    else
                    {
                        lotteryItems[i].Flags = "0";
                        lotteryItems[i].Effect = "0";
                    }

                    lotteryItems[i].Show = "0";
                    if(!(LotteryCollection.TreasureFitment == null))
                    {
                        for(int fitcount=0; fitcount < LotteryCollection.TreasureFitment.Length;fitcount++)
                        {
                            if(lotteryItems[i].ManID == LotteryCollection.TreasureFitment[fitcount].ManID)
                            {
                                lotteryItems[i].Show = "1";
                            }
                        }
                    }
                    lotteryItems[i].IsReset = "";
                    if(!(LotteryCollection.Reset== null))
                    {
                        for(int recount = 0; recount< LotteryCollection.Reset.Length;recount++)
                        {
                            if (lotteryItems[i].ManID == LotteryCollection.TreasureFitment[recount].ManID)
                            {
                                lotteryItems[i].IsReset = "1";
                            }
                        }
                    }

                }
                if(type ==4)
                {
                    lotteryItems[i].Probability = dt.Rows[ManPoint.Y + i + 1][ManPoint.X + 3].ToString();
                    double result = double.Parse(lotteryItems[i].Probability);
                    int temp = (int)((result * 100) + 0.5);
                    probabilitySum += temp;
                    lotteryItems[i].Probability = temp.ToString();

                    lotteryItems[i].NumOrTime = dt.Rows[ManPoint.Y + i + 1][ManPoint.X + 4].ToString();
                    if (lotteryItems[i].NumOrTime == "永久")
                    {
                        lotteryItems[i].NumOrTime = "0";
                    }
                    else
                    {
                        lotteryItems[i].NumOrTime = Regex.Replace(lotteryItems[i].NumOrTime, @"[^0-9]+", "");
                    }
                }

                if(type == 1 || type == 2 || type == 3)
                {
                    double result = double.Parse(lotteryItems[i].Probability);
                    int temp = (int)((result * ProbaTimes) + 0.5);
                    probabilitySum += temp;
                    lotteryItems[i].Probability = temp.ToString();
                }
            }

            if(type ==1 || type ==2 || type ==3)
            {
                if(!(probabilitySum == 10000*ProbaTimes))
                {
                    MessageBox.Show("type = "+type.ToString()+" , 权重概率总和不等于10000", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if(type ==4 )
            {
                if (!(probabilitySum == 100))
                {
                    MessageBox.Show("type = 4 , 权重概率总和不等于10000", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            return lotteryItems;
        }


        //小灵通，第三页签
        private void GetPHSLotteryCollection()
        {
            if(IsPHSLotteryLoad)
            {
                //数据已经载入系统
                return;
            }
            
            //开始读取，确认第三个页签存在
            if(!isThirdDataTableExit)
            {
                phsLotteryCollection.ExchangeItems = null;
                phsLotteryCollection.OneBtnOpen = null;
                MessageBox.Show("读取“小灵通——跨服抽奖”需要相应页签存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Point point_ID = new Point(0, 0);
            point_ID = FileUtility.SearchColumn(ref thirdDataTable, point_ID, "小灵通——跨服抽奖");
            point_ID.Y = point_ID.Y + 2;
            point_ID = FileUtility.SearchRow(ref thirdDataTable, point_ID, "ID");

            int count = FileUtility.CountKey(ref thirdDataTable, point_ID);
            ExchangeItem[] exchangeItems = new ExchangeItem[count];
            for(int i = 0;  i<count; i++)
            {
                exchangeItems[i].ID = thirdDataTable.Rows[point_ID.Y + i + 1][point_ID.X].ToString();
                exchangeItems[i].Name = thirdDataTable.Rows[point_ID.Y + i + 1][point_ID.X - 1].ToString();
                exchangeItems[i].PriceDiamond = thirdDataTable.Rows[point_ID.Y + i + 1][point_ID.X + 1].ToString();
                exchangeItems[i].LimitMax = thirdDataTable.Rows[point_ID.Y + i + 1][point_ID.X + 2].ToString();
            }
            phsLotteryCollection.ExchangeItems = exchangeItems;


            point_ID = FileUtility.SearchRowNext(ref thirdDataTable, point_ID, "ID");
            count = FileUtility.CountKey(ref thirdDataTable, point_ID);
            ExchangeItem[] oneBtnOpens = new ExchangeItem[count];
            for (int i = 0; i<count; i++)
            {
                oneBtnOpens[i].ID = thirdDataTable.Rows[point_ID.Y + i + 1][point_ID.X].ToString();
                oneBtnOpens[i].Name = thirdDataTable.Rows[point_ID.Y + i + 1][point_ID.X - 1].ToString();
            }
            phsLotteryCollection.OneBtnOpen = oneBtnOpens;

            point_ID.X = point_ID.Y = 0;
            point_ID = FileUtility.SearchColumn(ref thirdDataTable, point_ID, "原PC端快捷购买对应的包裹：");
            phsLotteryCollection.QuickPurchase.Name = thirdDataTable.Rows[point_ID.Y+1][point_ID.X].ToString();
            phsLotteryCollection.QuickPurchase.ID = thirdDataTable.Rows[point_ID.Y + 1][point_ID.X+1].ToString();

            IsPHSLotteryLoad = true;
        }

        //分离日期
        private string[] GetDataAndTime(string Data , string Time = "")
        {
            string[] result = new string[2];
            if(!Data.Contains("/"))
            {
                Data = DateTime.FromOADate(Convert.ToInt32(Data)).ToString("d");
            }
            string[] data = Data.Split(' ');
            result[0] = data[0].Replace("/", "-");
            if(Time != "")
            {
                if(!Time.Contains("/"))
                {
                    Time = DateTime.FromOADate(Convert.ToDouble(Time)).ToString();
                }
                string[] time = Time.Split(' ');
                result[1] = time[1];     
            }
            return result;
        }

        //另存为功能
        private void SaveToOther(string fileName, string fileString)
        {
            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.Filter = ".xml文件(*.xml)|*.xml";
            SaveFile.Title = "另存为";
            SaveFile.RestoreDirectory = true;


            SaveFile.FileName = fileName;
            if (SaveFile.ShowDialog() == DialogResult.OK)
            {
                //保存文件
                File.WriteAllText(SaveFile.FileName, fileString, Encoding.Default);
            }
        }




        private void btnGetXlsxFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.Filter = "xls files (*.xls)|*.xls|xlsx files (*.xlsx)|*.xlsx";
            ofd.Title = "请选择导入excel文件";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                IsLotteryCollectionLoad = false;
                IsCostItemCollectionLoad = false;
                IsPHSLotteryLoad = false;
                IsTimeCollectionLoad = false;
                this.XlsxConfigFileText.Text = ofd.FileName;
                this.LogBox.Text = "";
            }
        }
        private void XlsxConfigFileTextDragDrop(object sender, DragEventArgs e)
        {
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();

            if (FileUtility.IsRightFile(path, "xls"))
            {
                IsLotteryCollectionLoad = false;
                IsCostItemCollectionLoad = false;
                IsPHSLotteryLoad = false;
                IsTimeCollectionLoad = false;
                this.XlsxConfigFileText.Text = path;
                this.LogBox.Text = "";
            }
            else
            {
                this.LogBox.Text = "拖入的输入excel文件应为xls或xlsx格式，请重新拖入！";
            }
        }
        private void XlsxConfigFileTextDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void btnGetGeneralConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.Filter = "*.xml(xml文件)|*.xml";
            ofd.Title = "请选择导出xml文件";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.GeneralConfigText.Text = ofd.FileName;
                this.LogBox.Text = "";
            }
        }
        private void GeneralConfigTextDragDrop(object sender, DragEventArgs e)
        {
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (FileUtility.IsRightFile(path, "xml"))
            {
                this.GeneralConfigText.Text = path;
                this.LogBox.Text = "";
            }
            else
            {
                this.LogBox.Text = "拖入的输出文件应为xml格式，请重新拖入！";
            }
        }
        private void GeneralConfigTextDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void btnGetLotteryConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.Filter = "*.xml(xml文件)|*.xml";
            ofd.Title = "请选择导出xml文件";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.LotteryConfigText.Text = ofd.FileName;
                this.LogBox.Text = "";
            }
        }
        private void LotteryConfigTextDragDrop(object sender, DragEventArgs e)
        {
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (FileUtility.IsRightFile(path, "xml"))
            {
                this.LotteryConfigText.Text = path;
                this.LogBox.Text = "";
            }
            else
            {
                this.LogBox.Text = "拖入的输出文件应为xml格式，请重新拖入！";
            }
        }
        private void LotteryConfigTextDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void btnGetFitmentsConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.Filter = "*.xml(xml文件)|*.xml";
            ofd.Title = "请选择导出xml文件";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.FitmentsConfigText.Text = ofd.FileName;
                this.LogBox.Text = "";
            }
        }
        private void FitmentsConfigTextDragDrop(object sender, DragEventArgs e)
        {
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (FileUtility.IsRightFile(path, "xml"))
            {
                this.FitmentsConfigText.Text = path;
                this.LogBox.Text = "";
            }
            else
            {
                this.LogBox.Text = "拖入的输出文件应为xml格式，请重新拖入！";
            }
        }
        private void FitmentsConfigTextDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void btnGetMobileConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.Filter = "*.xml(xml文件)|*.xml";
            ofd.Title = "请选择导出xml文件";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.MobileConfigText.Text = ofd.FileName;
                this.LogBox.Text = "";
            }
        }
        private void MobileConfigTextDragDrop(object sender, DragEventArgs e)
        {
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (FileUtility.IsRightFile(path, "xml"))
            {
                this.MobileConfigText.Text = path;
                this.LogBox.Text = "";
            }
            else
            {
                this.LogBox.Text = "拖入的输出文件应为xml格式，请重新拖入！";
            }
        }
        private void MobileConfigTextDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void btnGetTimelinessConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.Filter = "*.xml(xml文件)|*.xml";
            ofd.Title = "请选择导出xml文件";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.TimelinessText.Text = ofd.FileName;
                this.LogBox.Text = "";
            }
        }
        private void TimelinessTextDragDrop(object sender, DragEventArgs e)
        {
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (FileUtility.IsRightFile(path, "xml"))
            {
                this.TimelinessText.Text = path;
                this.LogBox.Text = "";
            }
            else
            {
                this.LogBox.Text = "拖入的输出文件应为xml格式，请重新拖入！";
            }
        }
        private void TimelinessTextDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

    }
}
