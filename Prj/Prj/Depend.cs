using System;
using System.Collections.Generic;
using System.Text;
//
namespace Prj
{
    class Depend
    {
        List<Div>divs;//���ּ��ϵļ���

        bool isText;//�Ƿ��¼���ı�����[���ʵ�����ݵ����ԣ������������ı����л���]isText===true
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="text">�������Ե�����</param>
        public Depend(bool text)
        {
            divs = new List<Div>();
            isText=text;
        }
        /// <summary>
        /// ���һ�����ݼ�¼
        /// </summary>
        /// <param name="s">����ֵ</param>
        /// <param name="id">���ݼ�¼����</param>
        public void addRecord(string s,int id)
        {
            int i;//��¼���ֵ�����
            Div div=new Div("");//��¼����
            int num = divs.Count;
            for (i=0; i < num; i++)
            {
               div=divs[i];
               if (isText)//�ı�����
               {
                   if (div.getVal().Equals(s))//�ҵ�Ҫ��ӵĻ���
                   {
                       break;
                   }
               }
               else
               {
                   double val = Convert.ToDouble(div.getVal());
                   double addval = Convert.ToDouble(s);
                   if (Math.Abs(Math.Abs(addval - val) / val) < 0.01)//��Χ��
                   {
                       break;
                   }
               }
            }
            if (i == num)//û���ҵ���Ӧ�Ļ���
            {
                //�����µĻ���
                Div d = new Div(s);
                divs.Add(d);
                //����µļ�¼
                d.addRec(id);
            }
            else
            {
                //����µļ�¼
                div.addRec(id);
            }
        }
        /// <summary>
        /// ��ȡ��ϵ������
        /// </summary>
        /// <param name="dec">���߻�����������</param>
        /// <param name="allNum">��������</param>
        /// <returns>������</returns>
        public double getDependance(Depend dec,int allNum)
        {
            double sum=0;//���������
            int divNum = divs.Count;//��ǰ���ֵĸ���
            List<Div> decDivs=dec.divs;//�������ԵĻ���
            int decDivNum = decDivs.Count;//���߻��ֵĸ���

            for (int i = 0; i < divNum; i++)//�������ּ���
            {
                Div keyDiv = divs[i];
                for (int j = 0; j < decDivNum; j++)
                {
                    Div decDiv = decDivs[j];
                    if (decDiv.containDiv(keyDiv))
                    {
                        sum += keyDiv.getRecNum();
                        break;
                    }
                }
            }
            return sum / allNum;
        }
    }
}
