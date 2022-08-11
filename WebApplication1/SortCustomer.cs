using System.Collections.Generic;
using System.Linq;

namespace WebApplication1
{
    public class SortCustomer
    {
        /// <summary>
        /// 新增二分排序
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="newCustomer"></param>
        public static void HalfSort(List<Customer> customers,Customer newCustomer) 
        {
            if (customers.Count == 0) 
            {
                customers.Add(newCustomer);
                return;
            }
            if (NoSort(customers, newCustomer))//特殊判断
                return;
            int first = 0;
            int end = customers.Count;
            int insertIndex = 0;
            while (first <= end)//查找到两边
            {
                int centreIndex = (first + end) / 2;
                if (customers[centreIndex].Score > newCustomer.Score)
                {
                    end = centreIndex - 1;
                }
                else if (customers[centreIndex].Score < newCustomer.Score)
                {
                    first = centreIndex + 1;
                }
                else if (newCustomer.Score == customers[centreIndex].Score) 
                {
                    int firstIndex = customers.First(p => p.Score == newCustomer.Score).Rank - 1;
                    int endIndex= customers.Last(p => p.Score == newCustomer.Score).Rank - 1;
                    List<Customer> sameScoreCustomer=customers.Skip(firstIndex).Take(endIndex- firstIndex).ToList();//获取相同分数的Customer
                    HalfSortSameScore(sameScoreCustomer, newCustomer);//把新加的插入进来
                    customers.InsertRange(firstIndex, sameScoreCustomer);
                    UpdateRank(customers, firstIndex, newCustomer.Rank);
                    return;
                }
            }
            insertIndex = (first + end) / 2 + 1;
            customers.Insert(insertIndex, newCustomer);
            UpdateRank(customers, insertIndex, newCustomer.Rank);
        }
        /// <summary>
        /// 二分排序相同分数的Customer
        /// </summary>
        /// <param name="sameScoreCustomer"></param>
        /// <param name="newCustomer"></param>
        private static void HalfSortSameScore(List<Customer> sameScoreCustomer, Customer newCustomer) 
        {
            int first = 0;
            int end = sameScoreCustomer.Count;
            int insertIndex = 0;
            while (first <= end)//查找到两边
            {
                int centreIndex = (first + end) / 2;
                if (sameScoreCustomer[centreIndex].CustomerId > newCustomer.CustomerId)
                {
                    end = centreIndex - 1;
                }
                else 
                {
                    first = centreIndex + 1;
                }
            }
            insertIndex = (first + end) / 2 + 1;
            sameScoreCustomer.Insert(insertIndex, newCustomer);
        }
        /// <summary>
        /// 更新Rank排序
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="startIndex"></param>
        private static void UpdateRank(List<Customer> customers,int insertIndex,int oldIndex) 
        {
            int startIndex = insertIndex > oldIndex ? oldIndex : insertIndex;//谁小就从哪更新
            for (int i = startIndex; i < customers.Count; i++)
            {
                customers[i].Rank = i + 1;
            }
        }
        /// <summary>
        /// 特殊判断
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="newCustomer"></param>
        public static bool NoSort(List<Customer> customers, Customer newCustomer) 
        {
            if (customers.First().Score < newCustomer.Score) 
            {
                customers.Insert(0, newCustomer);
                UpdateRank(customers, 0, newCustomer.Rank);//更新所有
                return true;
            }
            if (customers.Last().Score > newCustomer.Score) 
            {
                customers.Insert(customers.Count, newCustomer);
                return true;
            }
            return false;
        }
    }
}
