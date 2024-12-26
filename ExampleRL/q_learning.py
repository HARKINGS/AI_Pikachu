import gym
import keyboard  # Thư viện để kiểm tra phím nhấn
import pygame, sys
import numpy as np
import pickle

# Tải dữ liệu model từ file model_data.pkl
with open("./ExampleRL/model_data.pkl", "rb") as file:
    q_table = pickle.load(file)  # Tải trực tiếp Q-table

# Tạo biến môi trường
env = gym.make('MountainCar-v0', render_mode='human')  # 'human' để hiển thị cửa sổ render

# kích thước mỗi khoảng
q_table_segment_size = (env.observation_space.high - env.observation_space.low) / q_table.shape[:2]

# Hàm chuyển đổi state thành chỉ mục Q-table (giống như lúc training)
def convert_state(real_state):
    index = (real_state - env.observation_space.low) // q_table_segment_size
    return tuple(index.astype(np.int_))

current_state, _ = env.reset()
current_state = convert_state(current_state)
done = False

# Model tự chơi
while not done:
    # Chọn hành động tốt nhất dựa trên Q-table
    action = np.argmax(q_table[current_state])

    # Thực hiện hành động
    next_state, _, terminated, truncated, _ = env.step(action=action)

    # Cập nhật trạng thái hiện tại
    current_state = convert_state(next_state)

    # Hiển thị
    env.render()

    # Kiểm tra kết thúc
    done = terminated or truncated

# Kết thúc
env.close()