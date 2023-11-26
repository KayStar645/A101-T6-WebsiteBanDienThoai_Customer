from mlxtend.preprocessing import TransactionEncoder
from mlxtend.frequent_patterns import fpgrowth
import pandas as pd
import json
import sys

def run_fpgrowth(data, support, top_k):
    # Kiểm tra xem data có phải là list không
    if not isinstance(data, list):
        raise ValueError("Invalid data format. Expecting a list.")

    te = TransactionEncoder()
    te_ary = te.fit(data).transform(data)
    df = pd.DataFrame(te_ary, columns=te.columns_)
    frequent_itemsets = fpgrowth(df, min_support=support, use_colnames=True)

    # Sắp xếp theo sự xuất hiện và lấy top k
    frequent_itemsets = frequent_itemsets.sort_values(by='support', ascending=False).head(top_k)

    # Chuyển kết quả thành danh sách các itemset và số lần xuất hiện (%)
    result = []
    for _, row in frequent_itemsets.iterrows():
        result.append({'Itemset': list(row['itemsets']), 'SupportPercent': row['support'] * 100})

    return json.dumps(result)

if __name__ == "__main__":
    if len(sys.argv) != 4:
        print("Usage: FP-Growth.py <data_str> <min_support> <top_k>")
        sys.exit(1)

    # Lấy tham số từ dòng lệnh
    data_str = sys.argv[1]
    support = float(sys.argv[2])
    top_k = int(sys.argv[3])

    # Chuyển đổi chuỗi JSON thành danh sách Python
    data = json.loads(data_str)

    # Gọi hàm và in kết quả
    result = run_fpgrowth(data, support, top_k)
    print(result)