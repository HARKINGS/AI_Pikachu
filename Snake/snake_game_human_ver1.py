import pygame, sys, random

    # Khởi tạo màn hình trò chơi

# Định nghĩa các màu dùng trong chương trình
BLACK = (0, 0, 0)
WHITE = (255, 255, 255)
RED = (255, 0, 0)
YELLOW = (255, 255, 0)
GREEN = (0, 255, 0)
BLUE = (0, 0, 255)

# Khởi tạo pygame
pygame.init()

# Thiết lập chiều rộng và chiều cao của cửa sổ
screen_width = 640
screen_height = 480

# Thiết lập nội dung của cửa sổ
title = "Rắn săn mồi"

screen = pygame.display.set_mode((screen_width, screen_height))
pygame.display.set_caption(title)


    # Tạo rắn

# Khai báo kích thước của 1 con rắn (hình vuông)
snake_block = 10

# Khai báo toạ độ bắt đầu của rắn trong game
x_head = screen_width / 2
y_head = screen_height / 2

# Khai báo xác định sự thay đổi của đầu rắn (di chuyển theo 4 hướng chăng ???)
x_head_change = 0
y_head_change = 0   

clock = pygame.time.Clock() # Đồng hồ đếm thời gian

    # Tạo thức ăn cho rắn với toạ độ ngẫu nhiên
def respawn_food():
    randFoodX = random.randrange(0, screen_width - snake_block)
    surplusFoodX = randFoodX % snake_block
    foodx = round(randFoodX - surplusFoodX)

    randFoodY = random.randrange(0, screen_height - snake_block)
    surplusFoodY = randFoodY % snake_block
    foody = round(randFoodY - surplusFoodY)

    return foodx, foody

    # Tăng độ dài rắn khi ăn được mồi
snake_list = [] # Lưu toạ độ các khối của rắn
snake_length = 1 # Độ dài của rắn

foodx, foody = respawn_food()
while (foodx, foody) in snake_list:
    respawn_food()
    
# Hàm hiển thị rắn
def show_snake(snake_block, snake_list):
    block = 1
    for x in snake_list:
        # vẽ hình chữ nhật kích thước snake_block x snake_block tại toạ độ x[0], x[1]
        if block == snake_length:
            pygame.draw.rect(screen, RED, [x[0], x[1], snake_block, snake_block])
        else:
            pygame.draw.rect(screen, BLACK, [x[0], x[1], snake_block, snake_block])
        block += 1

# Hàm hiển thị điểm
font = pygame.font.SysFont('Comic Sans MS', 25)
def show_score(score):
    text = font.render("Score: " + str(score), True, BLACK)
    screen.blit(text, (0, 0))


    # Sự kiện rắn đâm vào biên và hiện thông báo
arialFont = pygame.font.SysFont('arial', 30) # Khai báo font chữ là aril, cỡ chữ 30

# Hàm hiển thị thông báo
def message(msg, color):
    mess = arialFont.render(msg, True, color)
    textRect = mess.get_rect()
    textRect.center = (screen_width / 2, screen_height / 2)
    screen.blit(mess, textRect)


    # Khởi tạo game play

# Lập trình chuyển động của rắn
game_over = False
preEvent = None
while True:
    while game_over == True:
        screen.fill(WHITE)
        message("Game over! Click SPACE to try again", RED)
        pygame.display.update()

        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit()
                sys.exit()
            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_SPACE:
                    game_over = False
                    x_head = screen_width / 2
                    y_head = screen_height / 2
                    x_head_change = 0
                    y_head_change = 0
                    snake_list = []
                    snake_length = 1
                    preEvent = None
                    foodx, foody = respawn_food()

    if (x_head >= screen_width) or (x_head < 0) or (y_head >= screen_height) or (y_head < 0):
        game_over = True
        continue

    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            pygame.quit()
            sys.exit()
        if event.type == pygame.KEYDOWN:
            if preEvent == pygame.K_UP or preEvent == pygame.K_DOWN or preEvent == None:
                if event.key == pygame.K_LEFT:
                    x_head_change = -snake_block
                    y_head_change = 0
                elif event.key == pygame.K_RIGHT:
                    x_head_change = snake_block
                    y_head_change = 0
            if preEvent == pygame.K_LEFT or preEvent == pygame.K_RIGHT or preEvent == None:
                if event.key == pygame.K_UP:
                    x_head_change = 0
                    y_head_change = -snake_block
                elif event.key == pygame.K_DOWN:
                    x_head_change = 0
                    y_head_change = snake_block
            preEvent = event.key

    screen.fill(WHITE)
    show_score(snake_length - 1)

    pygame.draw.rect(screen, BLUE, [foodx, foody, snake_block, snake_block])

    x_head += x_head_change
    y_head += y_head_change

    snake_head = []
    snake_head.append(x_head)
    snake_head.append(y_head)
    snake_list.append(snake_head)

    if len(snake_list) > snake_length:
        del snake_list[0]

    show_snake(snake_block, snake_list)

    # Rắn cắn đuôi là game over
    for block_snake in snake_list[:-1]:
        if block_snake == snake_head:
            game_over = True
            # pygame.quit()
            # sys.exit()
    
    # Ăn thức ăn thì mất đi, thức ăn mới xuất hiện ở vị trí khác
    if x_head == foodx and y_head == foody:
        snake_length += 1
        foodx, foody = respawn_food()

    pygame.display.update() # Cập nhật
    clock.tick(20) # Tốc độ di chuyển của rắn