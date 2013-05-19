using System;
using System.Collections.Generic;
using System.Text;

//
namespace Prj
{
    class DiffMatrixElem
    {
        int i;//���Ƚ�������ѵ����������
        int j;//�Ƚ�������ѵ����������
        List<int> diffAttrs;//������������Լ����������ļ���
        List<double> diffs;//���̶�
        /// <summary>
        /// ��ʼ��������Ԫ��
        /// </summary>
        /// <param name="i">���Ƚ�������ѵ����������</param>
        /// <param name="j">�Ƚ�������ѵ����������</param>
        public DiffMatrixElem(int i, int j)
        {
            this.i = i;
            this.j = j;
            this.diffAttrs = new List<int>();
            this.diffs = new List<double>();
        }
        /// <summary>
        /// ���һ�������������
        /// </summary>
        /// <param name="attr">��������ֵ</param>
        /// <param name="diff">��������̶�</param>
        public void addDiffAttr(int attr,double diff)
        {
            diffAttrs.Add(attr);
            diffs.Add(diff);
        }
        /// <summary>
        /// Ϊһ������¼��Ȩ
        /// </summary>
        /// <param name="depend">�����ȼ���</param>
        /// <param name="expert">ר�Ҳ�������</param>
        /// <param name="weights">��Ȩֵ����Ҫ�ۼ�</param>
        public void weightedRec(List<double> depend, List<double> expert, List<double> weights)
        {
            int attrNum = diffAttrs.Count;//��¼���Եĸ���
            int keyNum=depend.Count;//���Ե��ܸ���
            //��ʱ��¼ÿ�����Ե�Ȩֵ            
            List<double> weight = new List<double>();
            for (int k = 0; k < keyNum; k++)
            {
                weight.Add(0);
            }
            double weightSum=0;//����Ȩֵ���ܺ�
            //����������Լ���,���Ȩֵ
            for (int k = 0; k < attrNum; k++)
            {
                int key = diffAttrs[k];
                key--;//����������ƫ��
                weight[key] = diffs[k] * depend[key] * expert[key];//Ȩֵ=����*������*ר�Ҳ���
                weightSum += weight[key];//�ۻ�
            }
            //��Ȩֵ�ۼ���һ��
            for (int k = 0; k < keyNum; k++)
            {
                double w=weight[k];
                if (w != 0)
                    weights[k] += w/weightSum;
            }
        }
        /// <summary>
        /// �ж�ĳ�������Ƿ��ھ���Ԫ����
        /// </summary>
        /// <param name="id">���Ե�����</param>
        /// <returns>�Ƿ����</returns>
        public bool hasAttr(int id)
        {
            int attrNum = diffAttrs.Count;//�������Ը���
            int k;
            for (k = 0; k < attrNum; k++)
            {
                if (diffAttrs[k] == id)
                    break;
            }
            return (k != attrNum && diffs[k] > 0.9);//���������Բ��ȴ���0.2
        }
        //public bool test()
        //{
        //    int num = diffAttrs.Count;
        //    for (int m = 0; m < num; m++)
        //    {
        //        if (diffs[m] > 0.4)
        //            return true;
        //    }
        //    return false;
        //}
        //public void write()
        //{
        //    if (diffAttrs.Contains(2))
        //        return;
        //    Console.Write(i + "," + j + ":");
        //    int num = diffAttrs.Count;
        //    for (int m = 0; m < num; m++)
        //    {
        //        Console.Write(diffAttrs[m]+"/");
        //    }
        //    Console.WriteLine();
        //}
    }
}
