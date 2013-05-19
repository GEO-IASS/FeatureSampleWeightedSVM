using System;
using System.Collections.Generic;
using System.Text;

namespace Prj
{
    class Div
    {
        string val;//��¼����
        List<int> recIds;//��¼��ֵͬ�ļ�¼����
        /// <summary>
        /// ���컮�ֶ���
        /// </summary>
        /// <param name="val">���컮�ָ��ݵ�ֵ</param>
        public Div(string val)
        {
            this.val = val;
            recIds = new List<int>();
        }
        /// <summary>
        /// ��ȡ���ֵļ���Ԫ�ظ���
        /// </summary>
        /// <returns>Ԫ�ظ���</returns>
        public int getRecNum()
        {
            return recIds.Count;
        }
        /// <summary>
        /// ��ȡ���컮�ֵĸ��ݵ�ֵ
        /// </summary>
        /// <returns>����ֵ</returns>
        public string getVal()
        {
            return val;
        }
        /// <summary>
        /// ���������һ����¼
        /// </summary>
        /// <param name="id">��¼����</param>
        public void addRec(int id)
        {
            recIds.Add(id);
        }
        /// <summary>
        /// �����Ƿ����ָ���ļ�¼
        /// </summary>
        /// <param name="id">ָ����¼����</param>
        /// <returns>�Ƿ��������</returns>
        bool hasId(int id)
        {
            int num = recIds.Count;//��¼��������
            for (int i = 0; i < num; i++)
            {
                if (id == recIds[i])//�ҵ�Ԫ��
                    return true;
            }
            return false;
        }
        /// <summary>
        /// �жϻ����Ƿ������һ������
        /// </summary>
        /// <param name="d">��һ������</param>
        /// <returns>�Ƿ��������</returns>
        public bool containDiv(Div d)
        {
            List<int> dRecIds = d.recIds;
            int num = dRecIds.Count;//���������ϵĸ���
            if (num > recIds.Count)//�������ļ����������ڰ������ϵĸ������϶�������
                return false;
            for (int i = 0; i < num; i++)//�����������ļ���
            {
                if (!hasId(dRecIds[i]))//��һ��Ԫ�ز��ڰ����ļ����У��϶�������
                    return false;
            }
            return true;
        }
    }
}
