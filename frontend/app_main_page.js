let commandQueue = [];

function createWebSocket() {
    let socket = new WebSocket('wss://chess.k6z.ru:8181');

    socket.onopen = function () {
        console.log('Соединение установлено');
        while (commandQueue.length > 0) {
            let command = commandQueue.shift();
            socket.send(command);
        }
    };

    socket.onerror = function (error) {
        console.error('Ошибка WebSocket:', error);
    };

    socket.onclose = function (event) {
        console.log('WebSocket закрыт. Повторная попытка подключения через 1 секунду...');
        setTimeout(() => {
            socket = createWebSocket();
        }, 1000);
    };

    socket.onmessage = function (event) {
        const data = event.data;

        if (data.includes("FEN:")) {
            const parts = data.slice(4).split(":");
            const newFEN = parts[0];
            const playerColor = parts[1];
            createChessboardFromFEN(newFEN, playerColor);
            switchTurn(); 
        } else if (data.includes("LOGS:")) {
            const logs = data.slice(5);
            logsField.innerHTML = logs.replace(/\n/g, '<br>');
        } else if (data.includes("GAMEID:")) {
            const gameId = data.slice(7);
            gameIdField.innerHTML = gameId;
            matchId = gameId;
            console.log('Получен matchId от сервера:', matchId);
        }
    };

    return socket;
}

let socket = createWebSocket();

function sendCommand(command) {
    if (socket.readyState === WebSocket.OPEN) {
        socket.send(command);
    } else {
        console.log('WebSocket не открыт. Команда добавлена в очередь.');
        commandQueue.push(command);
    }
}

const surrenderModal = document.getElementById('surrenderConfirmModal');
const drawOfferModal = document.getElementById('drawOfferModal');
const surrenderBtn = document.getElementById('surrender-btn');
const confirmSurrenderBtn = document.getElementById('confirm-surrender');
const cancelSurrenderBtn = document.getElementById('cancel-surrender');
const drawOfferBtn = document.getElementById('draw-offer-btn');
const acceptDrawBtn = document.getElementById('accept-draw');
const declineDrawBtn = document.getElementById('decline-draw');

surrenderBtn.addEventListener('click', function () {
    console.log('Surrender button clicked');
    surrenderModal.classList.remove('hidden');
});

drawOfferBtn.addEventListener('click', function () {
    console.log('Draw offer button clicked');
    drawOfferModal.classList.remove('hidden');
});

confirmSurrenderBtn.addEventListener('click', function() {
    console.log('Surrender confirmed');
    surrenderModal.classList.add('hidden');
});

cancelSurrenderBtn.addEventListener('click', function() {
    console.log('Surrender canceled');
    surrenderModal.classList.add('hidden');
});

acceptDrawBtn.addEventListener('click', function() {
    console.log('Draw accepted');
    drawOfferModal.classList.add('hidden');
});

declineDrawBtn.addEventListener('click', function() {
    console.log('Draw declined');
    drawOfferModal.classList.add('hidden');
});

const chessboard = document.getElementById('chessboard');
const files = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];
const ranksWhite = [8, 7, 6, 5, 4, 3, 2, 1];
const ranksBlack = [1, 2, 3, 4, 5, 6, 7, 8];
let selectedSquare = null;
let highlightedSquare = null; 
let matchId = null;

let whiteTime = 600; 
let blackTime = 600; 
let isWhiteTurn = true;
let timerInterval = null;

function startTimer() {
    if (timerInterval) return;

    timerInterval = setInterval(() => {
        if (isWhiteTurn) {
            whiteTime--;
            updateTimerDisplay('white', whiteTime);
        } else {
            blackTime--;
            updateTimerDisplay('black', blackTime);
        }

        if (whiteTime <= 0 || blackTime <= 0) {
            clearInterval(timerInterval);
            timerInterval = null;
            alert('Время вышло!');
        }
    }, 1000);
}

function switchTurn() {
    isWhiteTurn = !isWhiteTurn;
    if (!timerInterval) startTimer(); 
}

function updateTimerDisplay(player, time) {
    const minutes = Math.floor(time / 60);
    const seconds = time % 60;
    const timeString = `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
    document.querySelector(`.${player}-time`).textContent = timeString;
}

function updatePlayerLayout(playerColor) {
    const infoContainer = document.getElementById('info-container');
    const playerInfo = document.getElementById('player-info');
    const opponentInfo = document.getElementById('opponent-info');
    playerInfo.classList.remove('user-info1', 'user-info2');
    opponentInfo.classList.remove('user-info1', 'user-info2');

    if (playerColor === 'w') {
        infoContainer.appendChild(opponentInfo);
        infoContainer.appendChild(document.querySelector('.chessboard-container'));
        infoContainer.appendChild(playerInfo);
        opponentInfo.classList.add('user-info1');
        playerInfo.classList.add('user-info2');
    } else {
        infoContainer.appendChild(playerInfo);
        infoContainer.appendChild(document.querySelector('.chessboard-container'));
        infoContainer.appendChild(opponentInfo);
        playerInfo.classList.add('user-info1');
        opponentInfo.classList.add('user-info2');
    }
}

function createChessboardFromFEN(fen, playerColor) {
    updatePlayerLayout(playerColor);  

    chessboard.innerHTML = ''; 
    const ranks = playerColor === 'w' ? ranksWhite : ranksBlack;
    const files = playerColor === 'w' ? ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'] : ['h', 'g', 'f', 'e', 'd', 'c', 'b', 'a'];
    let position = fen.split(' ')[0];
    let rows = position.split('/');

    for (let i = 0; i < 64; i++) {
        const square = document.createElement('div');
        square.classList.add('square');
        chessboard.appendChild(square);

        const row = Math.floor(i / 8);
        const col = i % 8;

        if ((row + col) % 2 === 0) {
            square.classList.add('light');
        } else {
            square.classList.add('dark');
        }

        if (col === 0) {
            const rankLabel = document.createElement('span');
            rankLabel.textContent = ranks[row];
            rankLabel.classList.add('rank-label');
            square.appendChild(rankLabel);
        }

        if (row === 7) {
            const fileLabel = document.createElement('span');
            fileLabel.textContent = files[col];
            fileLabel.classList.add('file-label');
            square.appendChild(fileLabel);
        }

        addPieceFromFEN(square, row, col, rows);
        square.addEventListener('click', () => handleSquareClick(row, col, files, ranks, playerColor));
    }
}

function addPieceFromFEN(square, row, col, rows) {
    const fenRow = rows[row];
    let colIndex = 0;

    for (let char of fenRow) {
        if (!isNaN(char)) {
            colIndex += parseInt(char); 
        } else {
            const color = char === char.toUpperCase() ? 'white' : 'black';
            const piece = char.toLowerCase();
            if (colIndex === col) {
                const img = document.createElement('img');
                img.src = `reqs/${color}_${piece}.svg`; 
                img.classList.add('chess-piece');
                square.appendChild(img);
            }
            colIndex++;
        }
    }
}

function handleSquareClick(row, col, files, ranks, playerColor) {
    const clickedSquare = files[col] + ranks[row]; 

    if (highlightedSquare) {
        if (highlightedSquare.classList.contains('light')) {
            highlightedSquare.style.backgroundColor = '#efe6d5';
        } else if (highlightedSquare.classList.contains('dark')) {
            highlightedSquare.style.backgroundColor = 'rgba(60, 111, 111, 0.8)';
        }
        highlightedSquare.classList.remove('highlight');
    }
    
    const square = chessboard.querySelector(`.square:nth-child(${(row * 8) + col + 1})`);

    if (selectedSquare === null) {
        selectedSquare = clickedSquare;
        highlightedSquare = square;

        if (square.classList.contains('light')) {
            square.style.backgroundColor = '#d4c7b4';
        } else if (square.classList.contains('dark')) {
            square.style.backgroundColor = 'rgba(100, 151, 151, 1)';
        }

        square.classList.add('highlight');
        console.log('Selected square: ' + selectedSquare);
    } else {
        let move = `${selectedSquare}${clickedSquare}`; 
        console.log('Move: ' + move);

        sendCommand(`${matchId}:${move}`);

        selectedSquare = null;
        highlightedSquare = null;
    }
}

const commandInput = document.getElementById('commandInput');
const sendCommandButton = document.getElementById('sendCommand');
const logsField = document.getElementById('server_logs_field');

sendCommandButton.addEventListener('click', () => {
    const command = commandInput.value;
    if (command) {
        sendCommand(command);
        commandInput.value = '';
    }
});

const whiteFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
createChessboardFromFEN(whiteFEN, 'w');
