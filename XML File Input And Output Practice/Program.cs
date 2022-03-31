using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace XML_File_Input_And_Output_Practice
{
    internal class Program
    {
        static string fileFolderPath = @"C:\TestOutput";
        static void Main(string[] args)
        {
            //CreateXMLFile1();
            //CreateXMLFile2();
            //CreateXMLFile3();
            //CreateXMLFile4();
            //ReadXMLFile();
            //FindElement();
            //AddElement();
            //DeleteElement();
            ModifyElement();

            Console.ReadLine();
        }

        //使用第一种方法创建(格式不正确) XmlDocument
        public static void CreateXMLFile1()
        {
            List<Student> students = getStudents();
            //创建一个XML文档实例
            XmlDocument xmlDoc = new XmlDocument();
            //创建根节点
            XmlElement rootElement = xmlDoc.CreateElement("Students");
            //将根节点添加到文档中
            xmlDoc.AppendChild(rootElement);

            foreach (Student student in students)
            {
                //创建第一层节点
                XmlElement elementLevel1 = xmlDoc.CreateElement("Student");
                elementLevel1.InnerText = student.Name;
                elementLevel1.SetAttribute("Id", student.Id.ToString());
                //把第一层节点添加到根节点中
                rootElement.AppendChild(elementLevel1);

                //创建第二层节点
                XmlElement elementLevel2 = xmlDoc.CreateElement("Education");
                elementLevel2.InnerText = student.Education.School;
                elementLevel2.SetAttribute("Id", student.Education.Id.ToString());
                //把第二层节点添加到第一层节点中
                elementLevel1.AppendChild(elementLevel2);

                //创建第三层节点
                XmlElement elementLevel3 = xmlDoc.CreateElement("Degree");
                elementLevel3.InnerText = student.Education.Degree.Name;
                elementLevel3.SetAttribute("Level", student.Education.Degree.Level.ToString());
                //把第三层节点添加到第二层节点中
                elementLevel2.AppendChild(elementLevel3);
            }

            string outputPath = Path.Combine(fileFolderPath, "TestXmlOutput_1.xml");
            //保存并输出到指定路径
            xmlDoc.Save(outputPath);
            Console.WriteLine("Save xml file to " + outputPath);

        }

        //使用第二种方法创建（格式正确但太繁琐）XmlDocument
        public static void CreateXMLFile2()
        {
            List<Student> students = getStudents();
            //创建一个XML文档实例
            XmlDocument xmlDoc = new XmlDocument();
            //创建根节点
            XmlElement rootElement = xmlDoc.CreateElement("Students");
            //将根节点添加到文档中
            xmlDoc.AppendChild(rootElement);

            foreach (Student student in students)
            {
                //创建第一层节点
                XmlElement elementLevel1 = xmlDoc.CreateElement("Student");
                //elementLevel1.InnerText = student.Name;
                elementLevel1.SetAttribute("Id", student.Id.ToString());
                //把第一层节点添加到根节点中
                rootElement.AppendChild(elementLevel1);
                //把Name属性单独作为子节点添加到Student节点上
                XmlElement elementLevel2_Name = xmlDoc.CreateElement("Name");
                elementLevel2_Name.InnerText = student.Name;
                elementLevel1.AppendChild(elementLevel2_Name);

                //创建第二层节点
                XmlElement elementLevel2 = xmlDoc.CreateElement("Education");
                //elementLevel2.InnerText = student.Education.School;
                elementLevel2.SetAttribute("Id", student.Education.Id.ToString());
                //把第二层节点添加到第一层节点中
                elementLevel1.AppendChild(elementLevel2);
                //把School属性单独作为子节点添加到Education节点上
                XmlElement elementLevel3_School = xmlDoc.CreateElement("School");
                elementLevel3_School.InnerText = student.Education.School;
                elementLevel2.AppendChild(elementLevel3_School);

                //创建第三层节点
                XmlElement elementLevel3 = xmlDoc.CreateElement("Degree");
                //elementLevel3.InnerText = student.Education.Degree.Name;
                elementLevel3.SetAttribute("Level", student.Education.Degree.Level.ToString());
                //把第三层节点添加到第二层节点中
                elementLevel2.AppendChild(elementLevel3);
                //把Degree.Name属性单独作为子节点添加到Degree节点上
                XmlElement elementLevel4_Name = xmlDoc.CreateElement("Name");
                elementLevel4_Name.InnerText = student.Education.Degree.Name;
                elementLevel3.AppendChild(elementLevel4_Name);
            }

            string outputPath = Path.Combine(fileFolderPath, "TestXmlOutput_2.xml");
            //保存并输出到指定路径
            xmlDoc.Save(outputPath);
            Console.WriteLine("Save xml file to " + outputPath);

        }

        //使用第三种方法创建, 使用Linq to XML类，手动创建节点，手动嵌套进去
        public static void CreateXMLFile3()
        {
            List<Student> students = getStudents();
            //创建一个XML文档实例
            XDocument document = new XDocument();
            //创建根节点
            XElement rootElement = new XElement("Students");
            //将根节点添加到文档中
            document.Add(rootElement);

            foreach (Student student in students)
            {
                //第一层
                XElement stu = new XElement("Student");
                //直接为元素添加属性
                stu.SetAttributeValue("Id", student.Id);
                //直接为元素添加子节点
                stu.SetElementValue("Name", student.Name);

                //第二层
                XElement edu = new XElement("Education");
                edu.SetAttributeValue("Id", student.Education.Id);
                edu.SetElementValue("School", student.Education.School);

                //第三层
                XElement deg = new XElement("Degree");
                deg.SetAttributeValue("Level", student.Education.Degree.Level.ToString());
                deg.SetElementValue("Name", student.Education.Degree.Name);

                //把三层关系嵌套起来
                rootElement.Add(stu);
                stu.Add(edu);
                edu.Add(deg);
            }

            string outputPath = Path.Combine(fileFolderPath, "TestXmlOutput_3.xml");
            document.Save(outputPath);
            Console.WriteLine("Save xml file to " + outputPath);
        }

        //使用第四种方法创建, 使用Linq to XML类，直接在构造函数中创建节点（省去了节点之间嵌套的过程）
        public static void CreateXMLFile4()
        {
            List<Student> students = getStudents();
            //创建一个XML文档实例
            XDocument document = new XDocument();
            //创建根节点
            XElement rootElement = new XElement("Students");
            //将根节点添加到文档中
            document.Add(rootElement);

            foreach (Student student in students)
            {
                //第一层
                XElement stu = new XElement("Student",
                             //第二层
                             new XAttribute("Id", student.Id),
                                    new XElement("Name", student.Name),
                                    new XElement("Education",
                                        //第三层
                                        new XAttribute("Id", student.Education.Id),
                                               new XElement("School", student.Education.School),
                                               new XElement("Major",student.Education.Major),
                                               new XElement("Degree",
                                                    //第四层
                                                    new XAttribute("Level", student.Education.Degree.Level),
                                                           new XElement("Name", student.Education.Degree.Name)
                    )));
                //把创建的节点加入到根节点中
                rootElement.Add(stu);
            }

            string outputPath = Path.Combine(fileFolderPath, "TestXmlOutput_4.xml");
            document.Save(outputPath);
            Console.WriteLine("Save xml file to " + outputPath);
        }

        //从本地文件读取XML文档
        public static void ReadXMLFile()
        {
            string filePath = Path.Combine(fileFolderPath, "TestXmlOutput_4.xml");
            XDocument xdoc = XDocument.Load(filePath);
            //获取根元素
            XElement rootElement = xdoc.Root;
            //获取根元素下的子元素
            IEnumerable<XElement> elements = rootElement.Elements();
            //循环打印出每个子元素的信息
            foreach (XElement element in elements)
            {
                string StudentId = element.Attribute("Id").Value;
                string StudentName = element.Element("Name").Value;
                string SchoolId = element.Element("Education").Attribute("Id").Value;
                string SchoolName = element.Element("Education").Element("School").Value;
                string DegreeId = element.Element("Education").Element("Degree").Attribute("Level").Value;
                string DegreeName = element.Element("Education").Element("Degree").Element("Name").Value;
                Console.WriteLine("{0},{1},{2},{3},{4},{5}", StudentId, StudentName, SchoolId, SchoolName, DegreeId, DegreeName);
            }
        }

        //指定元素的Name，Attribute或 Value，找到特定元素
        public static void FindElement()
        {
            string filePath = Path.Combine(fileFolderPath, "TestXmlOutput_4.xml");
            XDocument xdoc = XDocument.Load(filePath);

            //根据元素的Name和Value找出特定元素
            XElement element1 = xdoc.Descendants()   //Descendants表示所有后代元素
                .Where(a => a.Name == "School" && a.Value == "RMIT")
                .FirstOrDefault();
            Console.WriteLine(element1);
            Console.WriteLine();

            //根据元素的Name和属性值找出特定的元素
            XElement element2 = xdoc.Descendants()    //Descendants表示所有后代元素
                .Where(a => a.Name == "Education" && a.Attribute("Id").Value == 5.ToString())
                .FirstOrDefault();
            Console.WriteLine(element2);
            Console.WriteLine();

            //指定层级，根据子元素找父元素
            XElement element3 = xdoc.Descendants()
                .Where(a => a.Name == "Name" && a.Value == "Masters Degree")
                .FirstOrDefault().Parent.Parent.Parent;
            Console.WriteLine(element3);
            Console.WriteLine();

            //指定祖先元素的Name，根据子元素找父元素
            XElement element4 = xdoc.Descendants()
                .Where(a => a.Name == "Name" && a.Value == "Masters Degree")
                .FirstOrDefault().Ancestors("Student").FirstOrDefault();
            Console.WriteLine(element4);
            Console.WriteLine();

            //根据元素的Index找到元素
            XElement element5 = xdoc.Root.Elements("Student").ElementAt(0);
            Console.WriteLine(element5);
            Console.WriteLine();
        }

        //添加元素
        public static void AddElement()
        {
            string filePath = Path.Combine(fileFolderPath, "TestXmlOutput_4.xml");
            XDocument xdoc = XDocument.Load(filePath);

            //添加元素
            Student s = new Student()
            {
                Id = 4,
                Name = "William",
                Education = new Education()
                {
                    Id = 18,
                    School = "ANU",
                    Degree = getDegreeByLevel(7),
                    Major = "Ecology"
                }
            };

            XElement xElement = new XElement("Student",
                              new XAttribute("Id", 4),
                                new XElement("Name", s.Name),
                                new XElement("Education",
                                    new XAttribute("Id", s.Education.Id),
                                    new XElement("School", s.Education.School),
                                    new XElement("Major", s.Education.Major),
                                    new XElement("Degree",
                                        new XAttribute("Id", s.Education.Degree.Level),
                                        new XElement("Name", s.Education.Degree.Name)
                                    )));
            xdoc.Root.Add(xElement);
            Console.WriteLine(xdoc);
            xdoc.Save(filePath);
            Console.WriteLine("Save complete.");
            Console.ReadKey();
        }

        //删除元素
        public static void DeleteElement()
        {
            string filePath = Path.Combine(fileFolderPath, "TestXmlOutput_4.xml");
            XDocument xdoc = XDocument.Load(filePath);
            XElement element=xdoc.Root.Elements("Student").ElementAt(0);
            element.Remove();
            xdoc.Save(filePath);
            Console.WriteLine("Delete complete.");
            Console.ReadKey();
        }

        //修改元素
        public static void ModifyElement()
        {
            string filePath = Path.Combine(fileFolderPath, "TestXmlOutput_4.xml");
            XDocument xdoc = XDocument.Load(filePath);
            XElement element = xdoc.Root.Elements("Student")
                .Where(a => a.Attribute("Id").Value == 3.ToString())
                .FirstOrDefault();
            element.SetElementValue("Name", "George");
            element.SetAttributeValue
            xdoc.Save(filePath);
            Console.WriteLine("Modify Complete");
            Console.ReadLine();
        }

        private static List<Student> getStudents()
        {

            Student s1 = new Student()
            {
                Id = 1,
                Name = "Mike",
                Education = new Education()
                {
                    Id = 15,
                    School = "RMIT",
                    Degree = getDegreeByLevel(6),
                    Major = "Accounting"
                }
            };
            Student s2 = new Student()
            {
                Id = 2,
                Name = "Jason",
                Education = new Education()
                {
                    Id = 5,
                    School = "Dicon",
                    Degree = getDegreeByLevel(7),
                    Major = "Chemistry"
                }
            };
            Student s3 = new Student()
            {
                Id = 3,
                Name = "Thomas",
                Education = new Education()
                {
                    Id = 17,
                    School = "Unimel",
                    Degree = getDegreeByLevel(9),
                    Major = "Architecture"
                }
            };
            List<Student> students = new List<Student>() { s1, s2, s3 };
            return students;
        }

        private static Degree getDegreeByLevel(int level)
        {
            Degree degree = new Degree();
            switch (level)
            {
                case 1:
                    degree.Level = 1;
                    degree.Name = "Certificate I";
                    break;
                case 2:
                    degree.Level = 2;
                    degree.Name = "Certificate II";
                    break;
                case 3:
                    degree.Level = 3;
                    degree.Name = "Certificate III";
                    break;
                case 4:
                    degree.Level = 4;
                    degree.Name = "Certificate IV";
                    break;
                case 5:
                    degree.Level = 5;
                    degree.Name = "Diploma";
                    break;
                case 6:
                    degree.Level = 6;
                    degree.Name = "Advanced Diploma, Associate Degree";
                    break;
                case 7:
                    degree.Level = 7;
                    degree.Name = "Bachelor Degree, Undergraduate Certificate";
                    break;
                case 8:
                    degree.Level = 8;
                    degree.Name = "Bachelor Honours Degree, Graduate Certificate, Graduate Diploma";
                    break;
                case 9:
                    degree.Level = 9;
                    degree.Name = "Masters Degree";
                    break;
                case 10:
                    degree.Level = 10;
                    degree.Name = "Doctoral Degree";
                    break;
                default:
                    degree.Level = 0;
                    degree.Name = "Null";
                    break;

            }
            return degree;
        }
    }



    class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Education Education { get; set; }
    }

    class Education
    {
        public int Id { get; set; }
        public string School { get; set; }
        public Degree Degree { get; set; }

        public string Major { get; set; }
    }

    class Degree
    {
        public int Level { get; set; }
        public string Name { get; set; }
    }
}
