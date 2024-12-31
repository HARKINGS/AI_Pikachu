import os
import torch
import random
import numpy as np
from collections import deque
from snake_model import Linear_QNet, QTrainer
from snake_game_ai import SnakeGameAI, Direction, Point
from helper import plot
from agent import Agent

env = SnakeGameAI()
state = env.reset()

# 1 state có 11 bit giá trị 
# 3 giá trị đầu chỉ độ nguy hiểm khi hướng di chuyển tiếp theo của đầu rắn
    # [1, 0, 0]: hướng di chuyển ko đổi, nguy hiểm
    # [0, 1, 0]: đầu rắn đi sang phải, nguy hiểm
    # [0, 0, 1]: đầu rắn đi sang trái, nguy hiểm
# 4 giá trị tiếp theo chỉ hướng di chuyển của rắn: trái, phải, trên, dưới
# 4 giá trị cuối chỉ vị trí của food: 4 bit có thể biểu diễn 8 giá trị
    # trên, dưới, trái, phải
    # trái trên, trái dưới, phải trên, phải dưới

agent = Agent()
game = SnakeGameAI()
plot_scores = []
plot_mean_scores = []
total_score = 0

# Load the model if it exists
if os.path.exists('./Snake/model/model.pth'):
    agent.load_model()
else:
    print("No pre-trained model found. Starting from scratch.")

while True:
    # get old state
    state_old = agent.get_state(game)

    # get move
    final_move = agent.get_action(state_old)

    # perform move and get new state
    reward, done, score = game.play_step(final_move)
    state_new = agent.get_state(game)

    # train short memory
    # agent.train_short_memory(state_old, final_move, reward, state_new, done)

    # remember
    # agent.remember(state_old, final_move, reward, state_new, done)

    if done:
        # train long memory, plot result
        game.reset()
        agent.n_games += 1
        # agent.train_long_memory()

        if score > agent.record:
            agent.record = score
            # agent.save_model()

        print('Game', agent.n_games, 'Score', score, 'Record:', agent.record)

        plot_scores.append(score)
        total_score += score
        mean_score = total_score / agent.n_games
        plot_mean_scores.append(mean_score)
        plot(plot_scores, plot_mean_scores)

        # Save the model at regular intervals
        # if agent.n_games % 10 == 0:
        #     agent.save_model('model_deep_q.pth')