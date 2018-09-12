# University of Washington, Programming Languages, Homework 6, hw6runner.rb

class MyTetris < Tetris
  def set_board
    @canvas = TetrisCanvas.new
    @board = MyBoard.new(self)
    @canvas.place(@board.block_size * @board.num_rows + 3,
                  @board.block_size * @board.num_columns + 6, 24, 80)
    @board.draw
  end
  def key_bindings
    super
    @root.bind('u', proc {@board.rotate_180})
    @root.bind('c', proc {@board.cheat})
  end
end

class MyPiece < Piece
  def self.next_piece (board)
    MyPiece.new(All_My_Pieces.sample, board)
  end
  def self.cheat_piece (board)
    MyPiece.new([[[0, 0]]], board)
  end
  All_My_Pieces = [[[[0, 0], [1, 0], [0, 1], [1, 1]]],  # square (only needs one)
                   rotations([[0, 0], [-1, 0], [1, 0], [0, -1]]), # T
                   [[[0, 0], [-1, 0], [1, 0], [2, 0]], # long (only needs two)
                    [[0, 0], [0, -1], [0, 1], [0, 2]]],
                   rotations([[0, 0], [0, -1], [0, 1], [1, 1]]), # L
                   rotations([[0, 0], [0, -1], [0, 1], [-1, 1]]), # inverted L
                   rotations([[0, 0], [-1, 0], [0, -1], [1, -1]]), # S
                   rotations([[0, 0], [1, 0], [0, -1], [-1, -1]]),  # Z
                   [[[0, 0], [-1, 0], [1, 0], [2, 0], [-2, 0]], # x-long (only needs two)
                    [[0, 0], [0, -1], [0, 1], [0, 2], [0, -2]]],
                   rotations([[0, 0], [0, 1], [1, 0]]), #corner
                   rotations([[0, 0], [0, 1], [1, 0],[0, -1],[1, -1]])]  # fat corner
end

class MyBoard < Board
  Cost_Of_Cheating = 100

  def initialize (game)
    @grid = Array.new(num_rows) {Array.new(num_columns)}
    @current_block = MyPiece.next_piece(self)
    @score = 0
    @game = game
    @delay = 500
    #splitting cheating request and state helps allow serial cheat requests
    @cheat_requested = false
    @cheating = false
  end

  def next_piece
    if @cheat_requested
      #cheat requested -> move to cheating with cheat piece
      @cheat_requested = false
      @current_block =  MyPiece.cheat_piece(self)
      @cheating = true
    else
      #no cheat requested -> turn cheating off (should only be on in some cases), return normal piece
      @cheating = false
      @current_block =  MyPiece.next_piece(self)
    end
    @current_pos = nil
  end

  # had to override, replace hard-coded range piece size index
  def store_current
    locations = @current_block.current_rotation
    index_size = locations.size - 1
    displacement = @current_block.position
    (0..index_size).each{|index|
      current = locations[index]
      @grid[current[1]+displacement[1]][current[0]+displacement[0]] =
          @current_pos[index]
    }
    remove_filled
    @delay = [@delay - 2, 80].max
  end
  def cheat
    if !game_over? and @game.is_running?  and !@cheat_requested and @score >= Cost_Of_Cheating
      @cheat_requested = true
      @score -= Cost_Of_Cheating
    end
  end
  def rotate_180
    if !game_over? and @game.is_running?
      @current_block.move(0, 0, 2)
    end
    draw
  end
end
