import numpy as np
import random
import pygame
import pickle
from snake_game_ai import SnakeGameAI

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