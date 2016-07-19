$(document)
            .ready(function() {
                $('#message')
                    .keypress(function(e) {
                        if (e.keyCode == 13)
                            $('#sendmessage').click();
                    });
                PNotify.desktop.permission();
              
               
                
                
                
            });


var BOARD_INDEX = 10;
var SIZE = 40;
var game;
var ships = [];
var GAMEBOARD = document.getElementById('gameboard');
var DOCKS = document.getElementById('docks');

function showNotification(text) {
    (new PNotify({
        title: 'Notification',
        text: text,
        after_init: function (notice) {
            notice.attention('rubberBand');
        },
        addclass: "stack-modal",
        desktop: {
            desktop: document.hidden,
            icon: '/Content/Images/Sea-ship-war-planes_1600x1200.jpg)'
        }
    })).get().click(function (e) {
        if ($('.ui-pnotify-closer, .ui-pnotify-sticker, .ui-pnotify-closer *, .ui-pnotify-sticker *').is(e.target))
            return;
    });
}

function Ship(lifes, name) {
    this.lifes = lifes;
    this.position = new Position();
    this.isOnXAxis = true;
    this.name = name;
    this.hasBeenSet = false;
    this.Size = lifes;
}

function Position() {
    this.XPosition = 0;
    this.YPosition = 0;
}

function setMap() {
    if (ships.filter(s => !s.hasBeenSet)[0] == undefined) {
        $('#carrier').off();
        $('#battleship').off();
        $('#destroyer').off();
        $('#submarine').off();
        $('#patrol').off();
        $('#carrier').attr('draggable', false);
        $('#battleship').attr('draggable', false);
        $('#destroyer').attr('draggable', false);
        $('#submarine').attr('draggable', false);
        $('#patrol').attr('draggable', false);
        game.server.setMap(ships);
        $("#btnSetShips").hide();
    } else {
        showNotification("You must set all ships...");
        //$('#statusModal').text("You must set all ships...");
        //$('#notificationModal').modal('show');
    }

}

function startGame() {
    $("#actions").css("visibility", "hidden");
    GAMEBOARD.style.opacity = 0.5;
    GAMEBOARD.parentElement.className = 'col-md-6';
    var docks = document.getElementById('docks');
    var parent = docks.parentElement;
    parent.parentElement.removeChild(parent);
    parent.removeChild(docks);
    //GAMEBOARD
    buildFireBoard();
}

function setShips(ships) {
    ships.forEach(function(element) {
        const ship = document.getElementById(element.Name);
        const cell = document.getElementById(element.Position.YPosition + '-' + element.Position.XPosition);
        setShip(ship, cell);
    });
         
}

document.addEventListener('DOMContentLoaded', function () {

    createGame();
});

//$(function() {

//    // Reference the auto-generated proxy for the hub.
//    game = $.connection.gameHub;
//    game.client.wait = wait;
//    game.client.createGame = createGame;
//    // Create a function that the hub can call back to display messages.
//    game.client.addNewMessageToPage = addNewMessageToPage;
//    game.client.receiveHitResponse = receiveHitResponse;
//    game.client.beginTurn = beginTurn;
//    game.client.startGame = startGame;
//    game.client.renderHit = renderHit;
//    game.client.buildBoard = buildBoard;
//    game.client.buildFireBoard = buildFireBoard;
//    game.client.setShips = setShips;
//    // Get the user name and store it to prepend to messages.
//    $('#displayname').val('@User.Identity.GetUserName()');
//    // Set initial focus to message input box.
//    $('#message').focus();
//    // Start the connection.
//    $.connection.hub.start()
//        .done(function() {
//            $('#sendmessage')
//                .click(function() {
//                    // Call the Send method on the hub.
//                    game.server.send($('#displayname').val(), $('#message').val());
//                    // Clear text box and reset focus for next comment.
//                    $('#message').val('').focus();
//                });
//            game.server.joined();
//        });
//    $.connection.hub.disconnected(function() {
//        setTimeout(function() {
//            $.connection.hub.start();
//        },
//            2000);
//    });
//});

// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

function genCellId(j, i) {
    return '' + j + '-' + i;
};

function wait(status) {
    $("#waiter").css("visibility", "visible");
    if (status == null) {
        status = "Waiting for an opponent ...";
    }
    $("#status").text(status);
};

function createGame() {
    $("#waiter").css("visibility", "hidden");
    $("#actions").css("visibility", "visible");

    buildBoard();
    loadShips();
           
    var text = "Set your ships!!";

    showNotification(text);
};

function addNewMessageToPage(name, message) {
    // Add the message to the page.
    $('#discussion').append('<li><strong>' + htmlEncode(name) + '</strong>: ' + htmlEncode(message) + '</li>');
};

function buildBoard() {
    for (i = 0; i < BOARD_INDEX; i++) {
        for (j = 0; j < BOARD_INDEX; j++) {

            // create a new div HTML element for each grid square and make it the right size
            var square = document.createElement("div");
            AddCellTitle(square);
            GAMEBOARD.appendChild(square);

            // give each div element a unique id based on its row and column, like "s00"
            square.id = genCellId(j, i);

            // set each grid square's coordinates: multiples of the current row or column number
            var topPosition = j * SIZE;
            var leftPosition = i * SIZE; // use CSS absolute positioning to place each grid square on the page
            square.style.top = topPosition + 'px';
            square.style.left = leftPosition + 'px';
            square.className = 'cell normal';
            square.addEventListener('dragenter', drag_enter, false);
            square.addEventListener('dragleave', drag_leave, false);
            square.addEventListener('dragenter', drag_over, false);
            square.addEventListener('drop', drop, false);
            document.body.addEventListener('dragover', drag_over, false);
        }
    }
};

function buildFireBoard() {
    //document.removeChild(document.getElementById('docks'));
    var newBoard = document.getElementById('fireboard').children[0];
    newBoard.innerHTML = "";
    for (i = 0; i < BOARD_INDEX; i++) {
        for (j = 0; j < BOARD_INDEX; j++) {
            // create a new div HTML element for each grid square and make it the right size
            var square = document.createElement("div");
            AddCellTitle(square);
            newBoard.appendChild(square);

            // give each div element a unique id based on its row and column, like "s00"
            square.id = genCellId(j, i) + " f";

            // set each grid square's coordinates: multiples of the current row or column number
            var topPosition = j * SIZE;
            var leftPosition = i * SIZE;

            // use CSS absolute positioning to place each grid square on the page
            square.style.top = topPosition + 'px';
            square.style.left = leftPosition + 'px';
            square.className = 'cell normal';

            square.addEventListener('mouseenter', drag_enter, false);
            square.addEventListener('mouseleave', drag_leave, false);
            square.addEventListener('click', fire, false);
        }
    }
    newBoard.parentElement.parentElement.className = 'col-md-6';
};

function loadShips() {

    var carrier = document.createElement('div');
    carrier.id = 'carrier';
    var battleship = document.createElement('div');
    battleship.id = 'battleship';
    var destroyer = document.createElement('div');
    destroyer.id = 'destroyer';
    var submarine = document.createElement('div');
    submarine.id = 'submarine';
    var patrol = document.createElement('div');
    patrol.id = 'patrol';

    patrol.className = 'ships';
    submarine.className = 'ships';
    battleship.className = 'ships';
    destroyer.className = 'ships';
    carrier.className = 'ships';

    buildship(2, patrol, 1);
    buildship(3, submarine, 2);
    buildship(4, destroyer, 3);
    buildship(5, battleship, 4);
    buildship(6, carrier, 5);
};

function buildship(size, container, position) {

    var topPosition = position * SIZE;
    var leftPosition = SIZE;
    ships.push(new Ship(size, container.id));
    container.style.top = topPosition + 'px';
    container.style.left = leftPosition + 'px';
    container.style.width = size * SIZE + 'px';
    let image = '/Content/Images/ship.png';

    container.style.backgroundImage = "url('" + image + "')";


    container.style.backgroundSize = (size * SIZE) + "px" + " 40px ";
    container.draggable = true;
    container.style.zIndex = 1;
    DOCKS.appendChild(container);
    $('#' + container.id).on('dragstart', drag_start);

};


function rotate(event) {
    event.preventDefault();

    var ship = document.getElementById(event.target.id);
    if (validateRotation(ship)) {
        var shipInArray = ships.filter(s => s.name === event.target.id)[0];
        shipInArray.isOnXAxis = !shipInArray.isOnXAxis;
        var height = ship.style.height == "" ? "40px" : ship.style.height;
        var width = ship.style.width;

        ship.style.height = width;
        ship.style.width = height;
        ship.style.backgroundSize = ship.style.width + " " + ship.style.height;

        let horizontal = '/Content/Images/ship.png';

        let vertical = '/Content/Images/ship_vertical.png';

        if (getIntHeight(ship) > getIntWidth(ship))
            ship.style.backgroundImage = "url('" + vertical + "')";
        else
            ship.style.backgroundImage = "url('" + horizontal + "')";

    } else {
        showNotification("The Gameboard has it's limits; you must learn not to anger the Gameboard...");
        //$('#statusModal').text("The Gameboard has it's limits; you must learn not to anger the Gameboard...");
        //$('#notificationModal').modal('show');
    }
};

function drag_start(event) {
    var style = window.getComputedStyle(event.target, null);
    event.originalEvent.dataTransfer.setData("text/plain", event.target.id);
};

function drag_over(event) {
    event.preventDefault();
    return false;
};

function drag_enter(event) {
    event.preventDefault();
    var cell = document.getElementById(event.target.id);
    if (cell && cell.style.display != 'none')
        cell.className = 'cell hovered';
};

function drag_leave(event) {
    event.preventDefault();
    var cell = document.getElementById(event.target.id);
    if (cell && cell.style.display != 'none')
        cell.className = 'cell normal';
};

function drop(event) {
            
    const ship = document.getElementById(event.dataTransfer.getData("text/plain"));
    const cell = document.getElementById(event.target.id);
    setShip(ship, cell);

    event.preventDefault();
    return false;
};

function setShip(ship, cell) {

    cell.className = 'cell normal';

    if (evaluateDrop(ship, cell)) {
        var shipOnArray = ships.filter(s => s.name === ship.id)[0];

        shipOnArray.position.XPosition = cell.id.split('-')[1];
        shipOnArray.position.YPosition = cell.id.split('-')[0];

        //ship.style.background = 'orange';
        let horizontal = '/Content/Images/ship.png';
        let vertical = '/Content/Images/ship_vertical.png';



        var shipHeight = getIntHeight(ship);
        var shipWidth = getIntWidth(ship);
        ship.style.backgroundSize = shipWidth + "px " + shipHeight + "px";
        if (shipWidth > shipHeight) {
            ship.style.backgroundImage = "url('" + horizontal + "')";
            var cellsRemaining = (shipWidth / SIZE) - 1;
            var origin = cell.id.split('-');
            for (i = 1; i <= cellsRemaining; i++) {
                let cellI = document.getElementById(origin[0] + '-' + (parseInt(origin[1]) + i));
            }
        } else {
            ship.style.backgroundImage = "url('" + vertical + "')";
            var cellsRemaining = (shipHeight / SIZE) - 1;
            var origin = cell.id.split('-');
            for (i = 1; i <= cellsRemaining; i++) {
                let cellI = document.getElementById((parseInt(origin[0]) + i) + '-' + origin[1]);
            }

        }
        ship.style.backgroundRepeat = 'no-repeat';
        $('#' + ship.id).off('click').on('click', rotate);

        ship.style.top = 0;
        ship.style.left = 0;
        cell.appendChild(ship);

        handleShipPositioning(ship, ship.parentElement);

        shipOnArray.hasBeenSet = true;

    } else {
        showNotification("The Gameboard has it's limits; you must learn not to anger the Gameboard...");
        //$('#statusModal').text("The Gameboard has it's limits; you must learn not to anger the Gameboard...");
        //$('#notificationModal').modal('show');
    }
}

function getIntHeight(ship) {
    if (ship.style.height == '')
        return 40;
    return parseInt(ship.style.height.replace('px', ''));
};

function getIntWidth(ship) {
    if (ship.style.width == '')
        return 40;
    return parseInt(ship.style.width.replace('px', ''));
};

function validateDrop(ship, cell) {
    var result;

    var height = getIntHeight(ship);
    var width = getIntWidth(ship);

    var exists;
    if (height > width) {
        var size = (height / SIZE) - 1;
        var validString = cell.id.split('-')[1];
        exists = size + '-' + validString;
    } else {
        var size = (width / SIZE) - 1;
        var validString = cell.id.split('-')[0];
        exists = validString + '-' + size;
    }

    if (document.getElementById(exists))
        result = true;
    else result = false;

    return result;
};

function validateRotation(ship) {
    var result;

    var cell = ship.parentElement;

    var projectedHeight = getIntWidth(ship);
    var projectedWidth = getIntHeight(ship);

    var exists;
    if (projectedHeight > projectedWidth) {
        var size = (projectedHeight / SIZE) - 1;
        var validString = cell.id.split('-')[0];
        exists = (parseInt(validString) + size) + '-' + cell.id.split('-')[1];
    } else {
        var size = (projectedWidth / SIZE) - 1;
        var validString = cell.id.split('-')[1];
        exists = cell.id.split('-')[0] + '-' + (parseInt(validString) + size);
    }

    if (document.getElementById(exists))
        result = true;
    else result = false;

    return result;

};

function handleShipPositioning(ship, cell) {

    var size = (getIntWidth(ship) / SIZE) - 1;

    var position = cell.id.split('-');
    var exists = position[0] + '-' + (parseInt(position[1]) + size);

    if (!document.getElementById(exists)) {
        var width = ship.style.width;
        var height = ship.style.height;
        ship.style.height = width;
        ship.style.width = height;
        var shipInArray = ships.filter(s => s.name === event.target.id)[0];
        shipInArray.isOnXAxis = false;
    }

};

function evaluateDrop(ship, cell) {
    var position = cell.id.split('-');
    var size = (getIntWidth(ship) / SIZE) - 1;

    var analyzeXAxis = document.getElementById((parseInt(position[1]) + size) + '-' + position[0]);
    var analyzeYAxis = document.getElementById(position[1] + '-' + (parseInt(position[0]) + size));
    if ((ship.style.width > ship.style.height || ship.style.height == "") && analyzeXAxis) {
        return true;
    } else if ((ship.style.height > ship.style.width || ship.style.width == "") && analyzeYAxis) {
        return true;
    }

    return false;
};

function fire() {
    //TODO add call to sendHitToServer
    var cell = document.getElementById(event.target.id);
    var position = new Position();
    position.XPosition = cell.id.split('-')[1].split(' ')[0];
    position.YPosition = cell.id.split('-')[0];
    sendHitToServer(position);
};

function sunkShip() {
            
    $('#sinkEnemyDestroyerSound').trigger('play');
}

function renderHit(coordinates,isMyHit,image) {
    if (isMyHit) { //Dibujar respuesta al tiro que hizo el jugador

        document.getElementById(coordinates.YPosition + '-' + coordinates.XPosition + ' f')
            .style.backgroundImage = "url('" + image + "')";
        document.getElementById(coordinates
                .YPosition +
                '-' +
                coordinates.XPosition +
                ' f')
            .style.backgroundSize = "40px 40px";
    } else { //Dibujar tiro del enemigo


        var fill = document.createElement('div');
        //fill.style.backgroundColor = color;
        fill.style.backgroundSize = "40px 40px";
        fill.style.backgroundImage = "url('" + image + "')";
        fill.style.zIndex = 2;
        fill.style.top = "0px;";
        fill.style.left = "0px;";
        fill.className = "cell normal";
        document.getElementById(coordinates.YPosition + '-' + coordinates.XPosition).appendChild(fill);
    }
}

function receiveHitResponse(coordinates, isMyHit, wasHit, hasGameEnded, isShipSunken) {
    var color;
    var image;
    if (wasHit) {
        image = '/Content/Images/1466050855_Explosion.png';
        $('#explosionSound').trigger('play');
        if (isMyHit && isShipSunken) {
            showNotification("You sunk and enemy ship!!");
            //$("#statusModal").text("You sunk and enemy ship!!");
            //$('#notificationModal').modal('show');
            setTimeout(sunkShip, 1000);
                    
        }
    } else {
        image = '/Content/Images/1466050417_ksplash.png';
        $('#waterSound').trigger('play');
    }
    renderHit(coordinates, isMyHit,image);
    if (hasGameEnded) {
        if (isMyHit) { //Gano
            $("#statusModal").text("You win!!! Congratulations!!");
            $('#notificationModalContent').css("background-Color", "Green");

        } else { //Perdio
            $("#statusModal").text("You lost!!!");
            $('#notificationModalContent').css("background-Color", "Red");

        }
        $('#notificationModal').modal('show');
        $('#btnPlayAgain').show();
    } else if (isMyHit) {
        endTurn();
    }
};

function sendHitToServer(coordinates) {
    game.server.hit(coordinates);
}

function beginTurn() {
    $("#waiter").css("visibility", "hidden");
    //$("#statusModal").text("Its your turn now!!");
    //$('#notificationModal').modal('show');
    showNotification("Its your turn now!!");
}

function endTurn() {
    game.server.endTurn();
}

function AddCellTitle(square) {
    if (i == 0 && j == 0) {
        square.innerHTML = '<h1 class="left-cell-title"><span>0</span></h1><h1 class="center-cell-title" style="margin-top: -40px !important"><span>0</span></h1>'
    }
    else if (i == 0) {
        square.innerHTML = `
        <h1 class="left-cell-title">
            <span>${j}</span></h1>`;
    } else if (j == 0) {
        square.innerHTML = `
        <h1 class="center-cell-title">
            <span>${i}</span></h1>`;
    }
}
