- Goal (mục đích): Chiều dài của rắn là dài nhất
- Environment (môi trường): khu vực xung quanh, rắn, mồi
- Agent (máy): Con rắn
- State (trạng thái): Trạng thái gồm 3 thông tin:
    + Đầu của rắn hướng về đâu
    + Vị trí của food
    + Những vị trí nguy hiểm với con rắn (vd: nếu quay lên trên khoảng 2 bước chết, đó là nguy hiểm)
- Action (hành động): Có 4 loại: Lên, xuống, trái, phải
- Reward (phần thưởng): 
    + Sống sau khi di chuyển: +0.1
    + Chết: -1
    + Ăn food: +1
- Terminate state: Trạng thái dừng (Chết)