using System;
using System.Collections.Generic;
using System.Text;
//
using System.Data;
using System.IO;

namespace Prj
{
    class Process
    {
        DB db;//���ݿ����
        Filter filter;//ɸѡ����
        Reduct reduct;//Լ�����
        /// <summary>
        /// ��ʼ��ȫ��ϵͳ����
        /// </summary>
        public Process()
        {
            
        }
        void processData(DataTable dt)
        {
            DataColumnCollection attrs = dt.Columns;//��ȡ����
            int attrNum = attrs.Count;//��ȡ���Ը���
            int recNum = dt.Rows.Count;//��¼����
            DataRowCollection rows=dt.Rows;//��¼�м�¼
            for (int i = 0; i < attrNum; i++)
            {
                List<string> valSpace = new List<string>();//��¼ֵ�ռ�
                for (int j = 0; j < recNum; j++)
                {
                    Object obj = rows[j][i];
                    if (attrs[i].DataType.ToString().Equals("System.Double"))//��ֵ����
                    {
                        if (Convert.IsDBNull(obj))//��ֵ
                        {
                            rows[j][i] = 0;
                        }
                    }
                    else//�ı�����
                    {
                        if (Convert.IsDBNull(obj))//��ֵ
                        {
                            rows[j][i] = "";
                        }
                        if (valSpace.Contains(obj.ToString()))//δ������ֵ
                        {
                            rows[j][i] = valSpace.IndexOf(obj.ToString())+1;//�õ�����
                        }
                        else//������ֵ
                        {
                            valSpace.Add(obj.ToString());//�����ֵ
                            rows[j][i] = valSpace.IndexOf(obj.ToString())+1;//�õ�����
                            //string s=rows[j][i].ToString();
                        }
                    }
                }                
            }
            List<int> rmList = new List<int>();
            for (int i = 0; i < recNum; i++)
            {
                if (i >= 153 && i <= 560)//2��
                {
                    //if (((i - 153) % 8 % 2 != 0)
                    //    || (i - 153) % 8 == 0)
                    if(false)
                    {
                        rmList.Add(i);
                    }
                }
            }
            for (int i = rmList.Count-1; i >=0; i--)
            {
                rows.RemoveAt(rmList[i]);
            }
            
        }
        /// <summary>
        /// �԰���ID������ɸѡ�������ݰ���1/3 -- 2/3����Ϊ����������ѵ������
        /// %3==1Ϊѵ������
        /// @3!=1Ϊ��������
        /// </summary>
        /// <param name="test">������������</param>
        /// <param name="train">ѵ����������</param>
        /// <returns>���Լ���</returns>
        DataColumnCollection classify(List<DataRow> testCollection, List<DataRow> trainCollection,List<double>valLength,List<double>max)
        {
            db = new DB();
            //��ѯ����
//            DataTable dt = db.getDataTable(@"select * from Test  Where category=3 or category=4
//            order by category"); Where category !=3
//            DataTable dt = db.getDataTable(@"SELECT   WanZuanFangShi.WZFS, AZ01.JBDM, AZ01.WZJS - AZ01.SJJS AS Expr1, AZ01.WZCW AS zymdc, AZ01.WJFFDM, 
//                AZ20.DJSD1, AZ20.DJSD2, AZ20.HD, AZ20.HYJBDM, AZ20.YSMCDM, AZ20.ZHJSDM, AZ20.ZJYXDMD1, 
//                AZ20.LDND1
//FROM      AZ01 INNER JOIN
//                AZ20 ON AZ01.JH = AZ20.JH INNER JOIN
//                WanZuanFangShi ON AZ01.JH = WanZuanFangShi.JH
//WHERE   (AZ20.CW = 'ed') order by wzfs");
            DataTable dt = db.getDataTable(@"SELECT   WanZuanFangShi.WZFS, AZ01.JBDM, AZ01.WZJS - AZ01.SJJS AS Expr1, AZ01.WZCW, AZ01.WJFFDM, 
                AZ20.DJSD1, AZ20.DJSD2, AZ20.HD, AZ20.HYJBDM, AZ20.YSMCDM, AZ20.ZHJSDM, AZ20.ZJYXDMD1, 
                AZ20.LDND1
FROM      AZ01 INNER JOIN
                AZ20 ON AZ01.JH = AZ20.JH INNER JOIN
                WanZuanFangShi ON AZ01.JH = WanZuanFangShi.JH
WHERE   (AZ20.CW = 'ed') and  AZ01.WZCW=AZ01.ZYMDC
ORDER BY WanZuanFangShi.WZFS");
            this.processData(dt);
            DataColumnCollection ret = dt.Columns;
            //��¼��ֵ���� 
            int attrNum = ret.Count;
            List<double> min = new List<double>();
            for (int i = 0; i < attrNum-1; i++)
            {
                max.Add(double.MinValue);
                min.Add(double.MaxValue);
            }
            //������ȡ����
            int num = dt.Rows.Count;//��������
            File.WriteAllText("./test.txt", "");
            File.WriteAllText("./rslt.txt", "");
            for (int j = 0; j < attrNum; j++)
            {
                File.AppendAllText("./test.txt", ret[j].DataType.ToString()+ "\t");
            }
            File.AppendAllText("./test.txt", Environment.NewLine);
            for (int i = 0; i < num; i++)
            {
                DataRow data = dt.Rows[i];//����
                if (i % 10 != 0)//��ȡ��������
                {
                    if (!(i >= 80 && i <= 136
                        //|| i >= 190 && i <= 220
                        || i >= 550 && i <= 660))//ǿ��ɾ���������� 505
                    {
                        testCollection.Add(data);
                    }
                    
                    //trainCollection.Add(data);
                }
                else//��ȡѵ������
                {
                    trainCollection.Add(data);
                }
                //���ˢ�º������
                for (int j = 0; j < attrNum; j++)
                {
                    File.AppendAllText("./test.txt",data[j].ToString()+"\t");
                }
                File.AppendAllText("./test.txt", Environment.NewLine);
                //
                for (int j = 1; j < attrNum; j++)//���Ե�һ����������
                {
                    double val=double.Parse(data[j].ToString());//��ȡֵ
                    if (val > max[j-1])
                        max[j-1] = val;//������
                    if (val < min[j-1])
                        min[j-1] = val;//������
                }
            }
            for (int i = 0; i < attrNum-1; i++)
            {
                valLength.Add(max[i] - min[i]);
            }
            //��ȡ���Լ�
            return ret;
        }
        /// <summary>
        /// ������
        /// </summary>
        public void run()
        {
            List<DataRow> trainCollection = new List<DataRow>();//ѵ������
            List<DataRow> testCollection = new List<DataRow>();//��������
            List<double> valLength = new List<double>();//ֵ�ռ�
            List<double> max=new List<double> ();//���ֵ
            //��ȡ����������ѵ������
            DataColumnCollection attrCollection=classify(testCollection, trainCollection,valLength,max);
            //����ɸѡ����
            filter = new Filter(trainCollection, attrCollection);
            int testNum = testCollection.Count;//ѵ����������
            DataRow testSample;//��������
            double sucNum = 0;//���Գɹ���������
            for (int i = 0; i < testNum; i++)//������������
            {
                
                //���������������͹ؼ����Թ���Լ�����
                reduct = new Reduct(trainCollection,attrCollection);
                List<double> dependance=reduct.calDependance();//������Ե�������
                DiffMatrix matrix=reduct.getDiffMatrix(valLength);//��ò�����
                //���ר�Ҳ���
                List<double> expertPara = new List<double>();//ר�Ҳ�������
                int attrNum=attrCollection.Count;
                for (int k = 0; k < attrNum-1; k++)
                {
                    expertPara.Add(1);
                }
                //Ϊ�ؼ����Լ�Ȩ
                List<double>weights=matrix.weightedKeyAttr(dependance, expertPara);
                //�������Զ���������
                List<int> sortedIds = matrix.sortWeight(weights);//����Ȩֵ��С���򣬼�¼�ؼ�����˳��
                //Լ������core��
                List<int>core=matrix.reduce(sortedIds);//����Ȩֵ˳��Լ������󣬻�ú��ļ���
                if (core.Count == 0)
                    continue;
                //��������
                testSample = testCollection[i];
                //�����Բ����������ƶ�������ֵ��ѵ�������������ƶ�
                Dictionary<int, double> id_u = filter.getIdSimilarity(testSample, valLength,weights,max,core);
                if (id_u.Count == 0)
                    continue;
                //���ö���SVM���о���
                Strategy strategy = new Strategy(id_u, weights,core);
                strategy.classify(trainCollection);
                if (strategy.predict(testSample))
                    sucNum++;
            }
            Console.WriteLine("׼ȷ��="+(int)(sucNum / testNum*10000)/100.0+"%");
        }
    }
}
