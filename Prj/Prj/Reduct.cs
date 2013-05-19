using System;
using System.Collections.Generic;
using System.Text;
//
using System.Data.SqlClient;
using System.Data;
namespace Prj
{
    class Reduct
    {
        List<DataRow> trainCollection;//ѵ����
        DataColumnCollection keyCollection;//�ؼ����Լ�
        public Reduct(List<DataRow> trainCollection, DataColumnCollection attrCollection)
        {
            this.trainCollection = trainCollection;
            keyCollection = attrCollection;
        }
        /// <summary>
        /// ��ȡ������
        /// </summary>
        public DiffMatrix getDiffMatrix(List<double>valLength)
        {
            DiffMatrix matrix=new DiffMatrix();//������
            int sampleNum = trainCollection.Count;//������������
            //���ѵ������
            for (int i = 0; i < sampleNum-1; i++)
            {
                DataRow first = trainCollection[i];
                for (int j = i + 1; j < sampleNum; j++)
                {
                    DataRow second = trainCollection[j];
                    //������
                    if (!first[0].ToString().Equals(second[0].ToString()))//�������Բ�ͬ
                    {
                        //����������Ԫ��
                        DiffMatrixElem elem = new DiffMatrixElem(i, j);
                        int keyNum = keyCollection.Count;
                        for (int k = 1; k < keyNum; k++)//��0�������Ǿ������ԣ����ǹؼ�����
                        {
                            //�ؼ�����
                            DataColumn keyAttr = keyCollection[k];
                            if (keyAttr.DataType.ToString().Equals("System.Single")
                                || keyAttr.DataType.ToString().Equals("System.Double")
                                || keyAttr.DataType.ToString().Equals("System.Int32"))
                            {
                                //��ȡ��������
                                double a = Convert.ToDouble(first[k]);
                                double x = Convert.ToDouble(second[k]);
                                //������̶�
                                //double diff = Math.Abs(Math.Abs(x - a) / a);
                                double diff = 0;
                                if (valLength[k-1] != 0)
                                    diff = Math.Abs(x - a) / valLength[k - 1];
                                if(diff>0)//������,û��Լ����Ϊ0
                                    elem.addDiffAttr(k, diff);//��Ӳ������
                            }
                            else
                            {
                                //�ı���Ԫ��
                                if (!first[k].ToString().Equals(second[k].ToString()))
                                {
                                    elem.addDiffAttr(k, 1);//��Ӳ������
                                }
                            }
                        }
                        //��Ӳ��Ԫ��
                        matrix.addElem(elem);
                    }
                }
            }
            //matrix.write();
            return matrix;
        }
        /// <summary>
        /// ����ؼ����Ե�������
        /// </summary>
        /// <returns>�����ȼ���1-13�洢��0-12</returns>
        public List<double> calDependance()
        {
            List<double> keyDependance=new List<double>();//�ؼ����Ե�������
            int sampleNum = trainCollection.Count;//������������
            //����������ԵĻ���
            Depend dec = new Depend(true);
            for (int j = 0; j < sampleNum; j++)//���������������������Ի���
            {
                dec.addRecord(trainCollection[j][0].ToString(), j);
            }
            int keyNum=keyCollection.Count;
            for (int i = 1; i < keyNum; i++)//�������������Ӧ��������
            {
                //����ؼ����ԵĻ���
                Depend depend = new Depend(true);//�������������ͣ�һ�ɰ����ı�����
                for (int j = 0; j < sampleNum; j++)//�����������������Ի���
                {
                    depend.addRecord(trainCollection[j][i].ToString(), j);
                }
                //���������
                keyDependance.Add(depend.getDependance(dec, sampleNum));
            }
            //for (int i = 0; i < keyDependance.Count; i++)
            //{
            //    Console.Write(keyDependance[i].ToString() + " ");
            //}
            return keyDependance;
        }
    }
}
