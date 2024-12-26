import pickle
import random
import gym
import numpy as np
import keyboard  # Thư viện để kiểm tra phím nhấn

# env = gym.make('MountainCar-v0', render_mode=None) # với render_mode = 'none' thì không hiển thị cửa sổ render
# env = gym.make('MountainCar-v0', render_mode='rgb_array') # với render_mode = 'none' thì không hiển thị cửa sổ render
env = gym.make('MountainCar-v0', render_mode='human')  # 'human' để hiển thị cửa sổ render, luôn render
initial_observation, _ = env.reset()

# Các tham số cần thiết

    # learning_rate: tốc độ học
c_learning_rate = 0.1

    # discount_factor: Thể hiện sự quan tâm tới tương lai
c_discount_factor = 0.95

# số lần training
c_no_of_eps = 100001

# cứ đạt được 1000 lần thì show 1 lần
c_show_each = 1000

    # tạo bảng q_table, chia vận tốc thành 20 đoạn từ 0.07 đến 0.07, 
    # chia vị trí thành 20 đoạn từ -1.2 đến 0.6
q_table_size = [20, 20]
    
    # kích thước mỗi khoảng
q_table_segment_size = (env.observation_space.high - env.observation_space.low) / q_table_size

# Hàm chuyển đổi từ real state sang index của q_table
def convert_state(real_state):
    index = (real_state - env.observation_space.low) // q_table_segment_size
    return tuple(index.astype(np.int_)) 
    # return tuple(int(x) for x in index)

# print(convert_state(initial_observation))

# Khởi tạo model rồi mới train model
# env.action_space.n: số lượng hành động
# env.action_space.sample(): chọn ngẫu nhiên một hành động
# lệnh đưới tạo q_table: ma trận 3 chiều, 
#                               2 chiều của q_table_size (state), 
#                               1 chiều của số hành động (action)
q_table = np.random.uniform(low=-2, high=0, size=(q_table_size + [env.action_space.n]))

max_ep_reward = -9999
max_ep_action_list = []
max_start_state = None

v_epsilon = 0.9
c_start_ep_epsilon_decay = 1
c_end_ep_epsilon_decay = c_no_of_eps // 2
v_epsilon_decay = v_epsilon / (c_end_ep_epsilon_decay - c_start_ep_epsilon_decay)

# Train model
for eps in range(c_no_of_eps):
    print("Episode: ", eps)
    done = False

    current_state, _ = env.reset()  # Lấy state từ kết quả của env.reset()
    current_state = convert_state(current_state)  # Chuyển đổi state thành chỉ mục của q_table
    
    ep_reward = 0
    action_list = []
    ep_start_state = current_state

    # Đạt điều kiện thoả mãn thì chuyển đổi render_mode
    if eps % c_show_each == 0:
        env = gym.make('MountainCar-v0', render_mode='human')
        show_now = True
    else:
        env = gym.make('MountainCar-v0', render_mode=None)
        show_now = False
    env.reset()

    # Vòng lặp cho mỗi episode, tìm hành động tốt nhất
    while not done:
        # Kiểm tra nếu phím 'q' được nhấn để thoát
        if keyboard.is_pressed('q'):
            print("Thoát chương trình.")
            exit()

        if random.random() > v_epsilon:
            # Lấy hành động từ Q-table
            action = np.argmax(q_table[current_state])
        else:
            action = random.randint(0, env.action_space.n - 1)
        action_list.append(action)

        # Thực hiện hành động và nhận phản hồi
        next_state, reward, terminated, truncated, info = env.step(action=action)
        ep_reward += reward

        # Render nếu đạt điều kiện (chỉ sau mỗi 1000 episode)
        if show_now:
            env.render()

        # done = True nếu terminated hoặc truncated
        if terminated or truncated:
            # Kiểm tra nếu đạt được mục tiêu
            if next_state[0] >= env.goal_position:
                print("Thành công đến cờ ep = {}, reward = {}".format(eps, ep_reward))
                if ep_reward > max_ep_reward:
                    max_ep_reward = ep_reward
                    max_ep_action_list = action_list
                    max_start_state = ep_start_state
                    # Lưu q_table
                    with open("model_data.pkl", "wb") as file:
                        pickle.dump(q_table, file)
            done = True
        else:
            # Chuyển đổi trạng thái tiếp theo thành index
            new_state = convert_state(next_state)

            # Cập nhật Q-value
            current_q_value = q_table[current_state + (action, )]
            new_q_value = (1 - c_learning_rate) * current_q_value + c_learning_rate * (reward + c_discount_factor * np.max(q_table[new_state]))
            q_table[current_state + (action, )] = new_q_value

            # Cập nhật trạng thái hiện tại
            current_state = new_state

    if c_end_ep_epsilon_decay >= eps >= c_start_ep_epsilon_decay:
        v_epsilon -= v_epsilon_decay

print("Max reward: ", max_ep_reward)
print("Max action list: ", max_ep_action_list)

env = gym.make('MountainCar-v0', render_mode='rgb_array')
# env = gym.make('MountainCar-v0', render_mode='human')
env.reset()
env.state = max_start_state
for action in max_ep_action_list:
    env.step(action)
    env.render()

done = False
while not done:
    _, _, done, _, _ = env.step(0)
    env.render()