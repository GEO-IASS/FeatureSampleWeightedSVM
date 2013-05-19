using System;
using System.Collections.Generic;
using System.Text;
//
using System.Data;
using System.IO;
namespace Prj
{
    class Filter
    {
        List<DataRow> trainCollection;//ѵ����
        DataColumnCollection filterCollection;//��¼ɸѡ����
        /// <summary>
        /// �������е����ݿ�����ѯɸѡ���Ե�����
        /// </summary>
        /// <param name="db">���е����ݿ����</param>
        public Filter(List<DataRow> trainCollection, DataColumnCollection filterCollection)
        {
            //��¼ѵ����
            this.trainCollection = trainCollection;
            //��¼ɸѡ����
            this.filterCollection = filterCollection;
        }
        /// <summary>
        /// ������������֮����ܲ��ȣ����ʵ���������ԣ�ȫ�������ı����ݴ�������ֵ��Ҫ��������
        /// </summary>
        /// <param name="testSample">��������</param>
        /// <param name="trainSample">ѵ������</param>
        /// <returns>��������֮����ܲ���</returns>
        double getDiffs(DataRow testSample, DataRow trainSample, List<double> valLength,List<double>weights,List<double>max,List<int>core)
        {
            double diffs = 0;
            //����ɸѡ���Լ�
//            int attrNum = filterCollection.Count;//���Ը���
            int attrNum = core.Count;//Լ������Ը���
            DataColumn filterAttr;//����
            for (int i = 0; i < attrNum; i++)
            {
                int k = core[i];
                //ɸѡ����
                filterAttr = filterCollection[k];
                if (filterAttr.DataType.ToString().Equals("System.Single")
                    || filterAttr.DataType.ToString().Equals("System.Double")
                    || filterAttr.DataType.ToString().Equals("System.Int32"))
                {
                    //��ȡ��������
                    double a = Convert.ToDouble(testSample[k]);
                    double x = Convert.ToDouble(trainSample[k]);
                    //������̶�
                    //��d����ͬ�ȷŴ󲻶Խ����Ӱ�� ��dƽ���Ŵ� �����ַ����Խϴ����Ч���ܺ�
                    //���ڻ��ӵ����� ��d���� Ч���ܺ�
                    double d = 1;
                    if (valLength[k - 1] != 0)
                    {
                        d = Math.Abs(x - a) / valLength[k - 1] * weights[k - 1];
                        //d = Math.Sqrt(d);
                        //d *= d;
                    }
                        //d = Math.Abs(x - a) / max[k - 1] * weights[k - 1];
                        //d = Math.Abs(x - a) / max[k - 1] / valLength[k - 1] * weights[k - 1];
                    diffs += d;
                }
                else
                {
                    //�ı��������ƶȶ�Ԫ��
                    if (testSample[filterAttr].ToString().Equals(trainSample[filterAttr].ToString()))
                        diffs+=weights[k-1];
                }
            }
            return diffs;
        }
        /// <summary>
        /// ���һ������������ѵ������֮������ƶ�
        /// </summary>
        /// <param name="testSample">��������</param>
        /// <param name="trainCollection">ѵ����������</param>
        /// <returns>�������ƶȼ���</returns>
        public Dictionary<int,double> getIdSimilarity(DataRow testSample,List<double>valLength,List<double>weights,List<double>max,List<int>core)
        {
            int trainNum = trainCollection.Count;//ѵ����������
            DataRow trainSample;//ѵ������
            Dictionary<int,double> id_u = new Dictionary<int,double> ();//���ƶȼ���
            List<double> allDiffs = new List<double> ();//����
            double maxDiffs = 0;//������
            int[] n = new int[3];
            //�������еĲ�𼯺�
            for (int i = 0; i < trainNum; i++)
            {
                trainSample = trainCollection[i];
                double diffs = getDiffs(testSample, trainSample,valLength,weights,max,core);//������ƶ�
                allDiffs.Add(diffs);
                if (maxDiffs < diffs)
                    maxDiffs = diffs;
            }
            for (int i = 0; i < trainNum; i++)
            {
                double similar=1-allDiffs[i]/ maxDiffs;
                if (similar >=0.0)
                {
                    id_u.Add(i, similar);
                    for (int j = 0; j < n.Length; j++)
                    {
                        if (trainCollection[i][0].ToString().Equals((j + 1).ToString()))
                        {
                            n[j]++;
                        }
                    }
                }
            }
            for (int i = 0; i < n.Length; i++)
            {
                //Console.Write(n[i] + " ");
                File.AppendAllText("./rslt.txt", n[i] + " ");
            }
            return id_u;
        }
    }
}
