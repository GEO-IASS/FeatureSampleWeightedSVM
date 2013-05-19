using System;
using System.Collections.Generic;
using System.Text;

namespace Prj
{
    class DiffMatrix
    {
        List<DiffMatrixElem> diff;//��¼������ķǿ���Ϣ
        /// <summary>
        /// ���������
        /// </summary>
        public DiffMatrix()
        {
            diff = new List<DiffMatrixElem>();
        }
        /// <summary>
        /// ��Ӿ���Ԫ��
        /// </summary>
        /// <param name="e">��ӵ�Ԫ��</param>
        public void addElem(DiffMatrixElem e)
        {
            diff.Add(e);
        }
        /// <summary>
        /// Ϊ�ؼ����Լ�Ȩ
        /// </summary>
        /// <param name="depend">�����ȼ���</param>
        /// <param name="expert">ר�Ҳ�������</param>
        /// <returns>�ؼ�����Ȩֵ����</returns>
        public List<double> weightedKeyAttr(List<double> depend, List<double> expert)
        {
            List<double> weights = new List<double>();
            int keyNum = depend.Count;//�ؼ����Ը���
            for (int i = 0; i < keyNum; i++)
            {
                weights.Add(0);
            }
            int elemNum = diff.Count;//����Ԫ�ظ���
            for (int i = 0; i < elemNum; i++)
            {
                diff[i].weightedRec(depend, expert, weights);
            }
            //Ȩֵ��һ��
            double maxWeight = 0;
            for (int i = 0; i < keyNum; i++)
            {
                if (maxWeight < weights[i])
                    maxWeight = weights[i];
            }
            for (int i = 0; i < keyNum; i++)
            {
                weights[i] /= maxWeight;
            }
            return weights;
        }
        /// <summary>
        /// ��������Ȩֵ����
        /// </summary>
        /// <param name="ws">����Ȩֵ����</param>
        /// <returns>����������˳��</returns>
        public List<int> sortWeight(List<double> ws)
        {
            //����Ȩֵ����
            List<double> weights = new List<double>();
            int keyNum = ws.Count;//�ؼ����Ը���
            List<int> weightSortKeys = new List<int>();//����������˳��
            for (int i = 0; i < keyNum; i++)
            {
                weightSortKeys.Add(i + 1);
                weights.Add(ws[i]);
            }
            //ѡ������            
            for (int i = 0; i < keyNum - 1; i++)
            {
                int k = i;
                int tmpId;
                double tmpWeight;
                for (int j = i + 1; j < keyNum; j++)
                {
                    if (weights[j] > weights[k])
                        k = j;
                }
                if (k != i)//�ҵ�����ģ�����
                {
                    //����Ȩֵ
                    tmpWeight = weights[i];
                    weights[i] = weights[k];
                    weights[k] = tmpWeight;
                    //��������
                    tmpId = weightSortKeys[i];
                    weightSortKeys[i] = weightSortKeys[k];
                    weightSortKeys[k] = tmpId;
                }
            }
            //for (int i = 0; i < keyNum; i++)
            //{
            //    Console.WriteLine(weights[i] + " -- " + weightSortKeys[i]);
            //}
            return weightSortKeys;
        }
        /// <summary>
        /// Լ��ؼ�����
        /// </summary>
        /// <param name="weights">��Ȩ�������Ȩֵ</param>
        /// <returns>core����</returns>
        public List<int> reduce(List<int> sortedIds)
        {
            List<int> core = new List<int>();//���ļ�
            int keyNum = sortedIds.Count;//�ؼ����Ը���
            for (int i = 0; i < keyNum; i++)
            {
                List<DiffMatrixElem> tmpDiff=new List<DiffMatrixElem>();
                int elemNum=diff.Count;//Ԫ�ظ���
                for (int j = 0; j < elemNum; j++)
                {
                    if (!diff[j].hasAttr(sortedIds[i]))//Լ��
                    {
                        tmpDiff.Add(diff[j]);
                    }
                }
                diff.Clear();
                diff = tmpDiff;
                if (diff.Count < elemNum)//Լ��ɹ�
                {
                    core.Add(sortedIds[i]);//���core��
                }
                //Console.WriteLine("ʣ��Ԫ�ظ���" + elemNum);
            }
            //for (int x = 0; x < core.Count; x++)
            //{
            //    Console.Write(core[x]+" ");
            //}
            return core;
            
            //return sortedIds;//����Լ��Ч����Ӱ�죬����ʹ��
        }
        //public void test()
        //{
        //    int sum = 0;
        //    for (int i = 0; i < diff.Count; i++)
        //    {
        //        if (!diff[i].test())
        //        {
        //            sum++;
        //            //diff[i].write();
        //        }
        //    }
        //    Console.WriteLine(sum);
        //}
        //public void write()
        //{
        //    for (int i = 0; i < diff.Count; i++)
        //    {
        //        diff[i].write();
        //        //System.Threading.Thread.Sleep(100);
        //    }
        //}
    }
}
