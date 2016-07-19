/**
 * Describes Board Position for Elements
 */
class BoardPosition {
    private xPosition: number;
    private yPosition: number;

    setXPosition(xValue : number) {
        this.xPosition = xValue;
    }

    getXPosition() : number {
        return this.xPosition;
    }

    setYPosition(yValue : number) {
        this.yPosition = yValue;
    }

    getYPosition() : number {
        return this.yPosition;
    }

    setCoordinates(xValue : number, yValue: number) {
        this.xPosition = xValue;
        this.yPosition = yValue;
    }
}

class Ship {
    SIZE: number = 40;
    private lifes: number;
    private position: BoardPosition;
    private isOnXAxis: boolean;
    private name: string;
    private hasBeenSet: boolean;
    private size: number;
    private element: HTMLElement;

    constructor(lifes : number, name : string) {
        this.element = document.createElement('div');
        this.element.id = name;
        this.element.className = 'ships';
        this.element.style.width = (lifes * this.SIZE) + 'px';

        let image = '/Content/Images/ship.png';
        this.element.style.backgroundImage = "url('" + image + "')";

        this.element.style.backgroundSize = (lifes * this.SIZE) + "px" + " 40px ";
        this.element.draggable = true;
        this.element.style.zIndex = '1';

        this.lifes = lifes;
        this.name = name;
        this.size = lifes;
        this.position = new BoardPosition();
    }

    addEvent(name: string, eventFunction: any, condition: boolean) {
        this.element.addEventListener(name, eventFunction, condition);
    }

    getElement(): HTMLElement {
        return this.element;
    }

    setElement(element: HTMLElement) {
        this.element = element;
    }

    getName() : string{
        return this.name;
    }

    setName(name: string) {
        this.name = name;
    }

    getLifes() {
        return this.lifes;
    }

    setLifes(lifes : number) {
        this.lifes = lifes;
    }

    getPosition() : BoardPosition {
        return this.position;
    }

    setPosition(position: BoardPosition) {
        this.position = position;
    }

    getIsOnXAxis() : boolean{
        return this.isOnXAxis;
    }

    setIsOnXAxis(isOnXAxis: boolean) {
        this.isOnXAxis = isOnXAxis;
    }

    getHasBeenSet() : boolean {
        return this.hasBeenSet;
    }

    setHasBeenSet(hasBeenSet: boolean) {
        this.hasBeenSet = hasBeenSet;
    }

    getSize() : number {
        return this.size;
    }

    setSize(size: number) {
        this.size = size;
    }

    setDockPosition(position: number) {
        this.element.style.top = (position * this.SIZE) + 'px';
        this.element.style.left = this.getLifes() + 'px';
    }
}

class Cell {
    SIZE: number = 40;
    element: HTMLElement;

    constructor(j: number, i: number) {
        this.element = document.createElement('div');
        this.element.id = this.genCellID(j, i);
        this.element.className = 'cell normal';

        var topPosition = j * this.SIZE;
        var leftPosition = i * this.SIZE; 
        this.element.style.top = this.genPosition(topPosition);
        this.element.style.left = this.genPosition(leftPosition);
    }

    getElement(): HTMLElement {
        return this.element;
    }

    setElement(element: HTMLElement) {
        this.element = element;
    }

    addEvent(name: string, eventFunction: any, condition: boolean) {
        this.element.addEventListener(name, eventFunction, condition);
    }

    private genCellID(j: number, i: number) : string {
        return '' + j + '-' + i;
    }

    private genPosition(position: number): string{
        return position + 'px';
    }
}

class Game {
    SIZE: number = 40;
    BOARD_INDEX : number = 10;
    GAMEBOARD: HTMLElement = document.getElementById('gameboard');
    DOCKS: HTMLElement = document.getElementById('docks');
    //game : Game;
    ships: Array<Ship> = new Array<Ship>();
    hub : any;

    constructor() {
        this.createGame();
        //Game = this;
    }

    setMap() {
        
    }

    setShips(ships: Array<Ship>) {
        ships.forEach(function (element : Ship) {
            const ship : HTMLElement = document.getElementById(element.getName());
            const cell : HTMLElement = document.getElementById(element.getPosition().getYPosition() + '-' + element.getPosition().getXPosition());
            this.setShip(ship, cell);
        });
    }

    setShip(ship: HTMLElement, cell: HTMLElement) {
        cell.className = 'cell normal';

        if (this.evaluateDrop(ship, cell)) {
            var shipOnArray = this.ships.filter(s => s.getName() === ship.id)[0];

            shipOnArray.getPosition().setXPosition(parseInt(cell.id.split('-')[1]));
            shipOnArray.getPosition().setYPosition(parseInt(cell.id.split('-')[0]));

            //ship.style.background = 'orange';
            let image = '/Content/Images/ship.png';

            ship.style.backgroundImage = "url('" + image + "')";

            var shipHeight = this.getIntHeight(ship);
            var shipWidth = this.getIntWidth(ship);
            ship.style.backgroundSize = shipWidth + "px " + shipHeight + "px";
            if (ship.style.width > ship.style.height) {
                var cellsRemaining = (shipWidth / this.SIZE) - 1;
                var origin = cell.id.split('-');
                for (var i : number = 1; i <= cellsRemaining; i++) {
                    let cellI = document.getElementById(origin[0] + '-' + (parseInt(origin[1]) + i));
                }
            } else {
                var cellsRemaining = (shipHeight / this.SIZE) - 1;
                var origin = cell.id.split('-');
                for (i = 1; i <= cellsRemaining; i++) {
                    let cellI = document.getElementById((parseInt(origin[0]) + i) + '-' + origin[1]);
                }

            }
            $('#' + ship.id).off('click').on('click', this.rotate);

            ship.style.top = '0';
            ship.style.left = '0';
            cell.appendChild(ship);

            this.handleShipPositioning(ship, ship.parentElement);

            shipOnArray.setHasBeenSet(true);

        } else {
            //showNotification("The Gameboard has it's limits; you must learn not to anger the Gameboard...");
            //$('#statusModal').text("The Gameboard has it's limits; you must learn not to anger the Gameboard...");
            //$('#notificationModal').modal('show');
        }
    }

    createGame() {
        $("#waiter").css("visibility", "hidden");
        $("#actions").css("visibility", "visible");

        this.buildBoard();
        this.loadShips();

        var text = "Set your ships!!";

        //this.showNotification(text);
    }

    buildBoard() {
        for (var i: number = 0; i < this.BOARD_INDEX; i++) {
            for (var j: number = 0; j < this.BOARD_INDEX; j++) {

                // create a new div HTML element for each grid square and make it the right size
                var square: Cell = new Cell(j, i);
                if (i == 0 && j == 0) {
                    square.getElement().innerHTML = '<h1 class="left-cell-title"><span>0</span></h1><h1 class="center-cell-title" style="margin-top: -40px !important"><span>0</span></h1>'
                }
                else if (i == 0) {
                    square.getElement().innerHTML = `
        <h1 class="left-cell-title">
            <span>${j}</span></h1>`;
                } else if (j == 0) {
                    square.getElement().innerHTML = `
        <h1 class="center-cell-title">
            <span>${i}</span></h1>`;
                }
                this.GAMEBOARD.appendChild(square.getElement());
                
                square.addEvent('dragenter', this.drag_enter, false);
                square.addEvent('dragleave', this.drag_leave, false);
                square.addEvent('dragenter', this.drag_over, false);
                square.addEvent('drop', this.drop, false);

                document.body.addEventListener('dragover', this.drag_over, false);
            }
        }
    }

    startGame() {
        $("#actions").css("visibility", "hidden");
        this.GAMEBOARD.style.opacity = "0.5";
        this.GAMEBOARD.parentElement.className = 'col-md-6';
        var docks = document.getElementById('docks');
        var parent = docks.parentElement;
        parent.parentElement.removeChild(parent);
        parent.removeChild(docks);

        this.buildFireBoard();
    }

    buildFireBoard() {
        //document.removeChild(document.getElementById('docks'));
        var newBoard = document.getElementById('fireboard').children[0];
        newBoard.innerHTML = "";
        for (var i = 0; i < this.BOARD_INDEX; i++) {
            for (var j = 0; j < this.BOARD_INDEX; j++) {
                // create a new div HTML element for each grid square and make it the right size
                var square: Cell = new Cell(j, i);
                if (i == 0 && j == 0) {
                    square.getElement().innerHTML = '<h1 class="left-cell-title"><span>0</span></h1><h1 class="center-cell-title" style="margin-top: -40px !important"><span>0</span></h1>'
                }
                else if (i == 0) {
                    square.getElement().innerHTML = `
        <h1 class="left-cell-title">
            <span>${j}</span></h1>`;
                } else if (j == 0) {
                    square.getElement().innerHTML = `
        <h1 class="center-cell-title">
            <span>${i}</span></h1>`;
                }
                newBoard.appendChild(square.getElement());

                // give each div element a unique id based on its row and column, like "s00"
                square.getElement().id = this.genCellId(j, i) + " f";

                // set each grid square's coordinates: multiples of the current row or column number
                var topPosition = j * this.SIZE;
                var leftPosition = i * this.SIZE;

                // use CSS absolute positioning to place each grid square on the page
                square.getElement().style.top = topPosition + 'px';
                square.getElement().style.left = leftPosition + 'px';
                square.getElement().className = 'cell normal';

                square.getElement().addEventListener('mouseenter', this.drag_enter, false);
                square.getElement().addEventListener('mouseleave', this.drag_leave, false);
                square.getElement().addEventListener('click', this.fire, false);
            }
        }
        newBoard.parentElement.parentElement.className = 'col-md-6';
    }

     genCellId(j, i) {
    return '' + j + '-' + i;
};

    loadShips() {

        var carrier: Ship = new Ship(6, 'carrier');
        carrier.setDockPosition(5)
        var battleship: Ship = new Ship(5, 'battleship');
        battleship.setDockPosition(4);
        var destroyer: Ship = new Ship(4, 'destroyer');
        destroyer.setDockPosition(3);
        var submarine: Ship = new Ship(3, 'submarine');
        submarine.setDockPosition(2);
        var patrol: Ship = new Ship(2, 'patrol');
        patrol.setDockPosition(1);
        
        this.addToDocks(patrol);
        this.addToDocks(submarine);
        this.addToDocks(destroyer);
        this.addToDocks(battleship);
        this.addToDocks(carrier);
    }

    addToDocks(ship: Ship) {
        this.ships.push(ship);
        
        this.DOCKS.appendChild(ship.getElement());
        $('#' + ship.getElement().id).on('dragstart', this.drag_start);
    }

    //Event Handlers defined
    rotate = (event) => {
        event.preventDefault();

        var ship = document.getElementById(event.target.id);
        if (this.validateRotation(ship)) {
            var shipInArray = this.ships.filter(s => s.getName() === event.target.id)[0];
            shipInArray.setIsOnXAxis(!shipInArray.getIsOnXAxis());
            var height = ship.style.height == "" ? "40px" : ship.style.height;
            var width = ship.style.width;

            ship.style.height = width;
            ship.style.width = height;
            ship.style.backgroundSize = height + "px " + width + "px";
        } else {
            //showNotification("The Gameboard has it's limits; you must learn not to anger the Gameboard...");
            //$('#statusModal').text("The Gameboard has it's limits; you must learn not to anger the Gameboard...");
            //$('#notificationModal').modal('show');
        }
    }

    drag_start = (event) =>  {
        var style = window.getComputedStyle(event.target, null);
        event.originalEvent.dataTransfer.setData("text/plain", event.target.id);
    }

    drag_over(event) {
        event.preventDefault();
        return false;
    }

    drag_leave(event) {
        event.preventDefault();
        var cell = document.getElementById(event.target.id);
        if (cell && cell.style.display != 'none')
            cell.className = 'cell normal';
    }

    drag_enter(event){
        event.preventDefault();
        var cell = document.getElementById(event.target.id);
        if (cell && cell.style.display != 'none')
            cell.className = 'cell hovered';
    }

    drop = (event) => {
        const ship = document.getElementById(event.dataTransfer.getData("text/plain"));
        const cell = document.getElementById(event.target.id);
        this.setShip(ship, cell);

        event.preventDefault();
        return false;
    }

    //Validation functions
    validateRotation(ship: HTMLElement) {
        var result;

        var cell = ship.parentElement;

        var projectedHeight = this.getIntWidth(ship);
        var projectedWidth = this.getIntHeight(ship);

        var exists;
        if (projectedHeight > projectedWidth) {
            var size = (projectedHeight / this.SIZE) - 1;
            var validString = cell.id.split('-')[0];
            exists = (parseInt(validString) + size) + '-' + cell.id.split('-')[1];
        } else {
            var size = (projectedWidth / this.SIZE) - 1;
            var validString = cell.id.split('-')[1];
            exists = cell.id.split('-')[0] + '-' + (parseInt(validString) + size);
        }

        if (document.getElementById(exists))
            result = true;
        else result = false;

        return result;
    }

    evaluateDrop(ship: HTMLElement, cell: HTMLElement) {
        var position = cell.id.split('-');
        var size = (this.getIntWidth(ship) / this.SIZE) - 1;

        var analyzeXAxis = document.getElementById((parseInt(position[1]) + size) + '-' + position[0]);
        var analyzeYAxis = document.getElementById(position[1] + '-' + (parseInt(position[0]) + size));
        if ((ship.style.width > ship.style.height || ship.style.height == "") && analyzeXAxis) {
            return true;
        } else if ((ship.style.height > ship.style.width || ship.style.width == "") && analyzeYAxis) {
            return true;
        }

        return false;
    }

    //Helpers
    getIntWidth(ship: HTMLElement) : number {
        if (ship.style.width == '')
            return 40;
        return parseInt(ship.style.width.replace('px', ''));
    }

    getIntHeight(ship: HTMLElement): number {
        if (ship.style.height == '')
            return 40;
        return parseInt(ship.style.height.replace('px', ''));
    }

    handleShipPositioning(ship: HTMLElement, cell: HTMLElement) {
        var size = (this.getIntWidth(ship) / this.SIZE) - 1;

        var position = cell.id.split('-');
        var exists = position[0] + '-' + (parseInt(position[1]) + size);

        if (!document.getElementById(exists)) {
            var width = ship.style.width;
            var height = ship.style.height;
            ship.style.height = width;
            ship.style.width = height;
            var shipInArray = this.ships.filter(s => s.getName() === ship.id)[0];
            shipInArray.setIsOnXAxis(false);
        }
    }

    //Playing

    fire(event) {
    var cell = document.getElementById(event.target.id);
    var position = new BoardPosition();
    position.setXPosition(parseInt(cell.id.split('-')[1].split(' ')[0]));
    position.setYPosition(parseInt(cell.id.split('-')[0]));
    this.sendHitToServer(position);
}

sunkShip() {
    $('#sinkEnemyDestroyerSound').trigger('play');
}

renderHit(coordinates, isMyHit, image) {
    if (isMyHit) { //Dibujar respuesta al tiro que hizo el jugador

        document.getElementById(coordinates.YPosition + '-' + coordinates.XPosition + ' f')
            .style.backgroundImage = "url('" + image + "')";
        document.getElementById(coordinates
            .YPosition +
            '-' +
            coordinates.XPosition +
            ' f')
            .style.backgroundSize = "40px 40px";
    } else {

        var fill = document.createElement('div');
        fill.style.backgroundSize = "40px 40px";
        fill.style.backgroundImage = "url('" + image + "')";
        fill.style.zIndex = "2";
        fill.style.top = "0px;";
        fill.style.left = "0px;";
        fill.className = "cell normal";

        document.getElementById(coordinates.YPosition + '-' + coordinates.XPosition).appendChild(fill);
    }
}

receiveHitResponse(coordinates, isMyHit, wasHit, hasGameEnded, isShipSunken) {
    var color;
    var image;
    if (wasHit) {
        image = '/Content/Images/1466050855_Explosion.png';
        $('#explosionSound').trigger('play');
        if (isMyHit && isShipSunken) {
            this.showNotification("You sunk and enemy ship!!");
            setTimeout(this.sunkShip, 1000);

        }
    } else {
        image = '/Content/Images/1466050417_ksplash.png';
        $('#waterSound').trigger('play');
    }
    this.renderHit(coordinates, isMyHit, image);
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
        this.endTurn();
    }
};

sendHitToServer(coordinates) {
    this.hub.server.hit(coordinates);
}

beginTurn() {
    $("#waiter").css("visibility", "hidden");
    this.showNotification("Its your turn now!!");
}

endTurn() {
    this.hub.server.endTurn();
}

}

document.addEventListener('DOMContentLoaded', function () {

    var game = new Game();
    // Reference the auto-generated proxy for the hub.
    game.hub = $.connection.gameHub;
    game.hub.client.wait = this.wait;
    game.hub.client.createhub = this.createhub;
    // Create a function that the hub can call back to display messages.
    game.hub.client.addNewMessageToPage = this.addNewMessageToPage;
    game.hub.client.receiveHitResponse = this.receiveHitResponse;
    game.hub.client.beginTurn = this.beginTurn;
    game.hub.client.starthub = this.starthub;
    game.hub.client.renderHit = this.renderHit;
    game.hub.client.buildBoard = this.buildBoard;
    game.hub.client.buildFireBoard = this.buildFireBoard;
    game.hub.client.setShips = this.setShips;
    // Get the user name and store it to prepend to messages.
    $('#displayname').val('@User.Identity.GetUserName()');
    // Set initial focus to message input box.
    $('#message').focus();
    // Start the connection.
    $.connection.gameHub.start()
        .done(function () {
            $('#sendmessage')
                .click(function () {
                    // Call the Send method on the hub.
                    game.hub.server.send($('#displayname').val(), $('#message').val());
                    // Clear text box and reset focus for next comment.
                    $('#message').val('').focus();
                });
            game.hub.server.joined();
        });
    $.connection.hub.disconnected(function () {
        setTimeout(function () {
            $.connection.hub.start();
        },
            2000);
    });
});