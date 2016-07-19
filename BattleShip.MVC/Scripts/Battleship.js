/**
* Describes Board Position for Elements
*/
class BoardPosition {
    setXPosition(xValue) {
        this.xPosition = xValue;
    }
    getXPosition() {
        return this.xPosition;
    }
    setYPosition(yValue) {
        this.yPosition = yValue;
    }
    getYPosition() {
        return this.yPosition;
    }
    setCoordinates(xValue, yValue) {
        this.xPosition = xValue;
        this.yPosition = yValue;
    }
}
class Ship {
    constructor(lifes, name) {
        this.SIZE = 40;
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
    addEvent(name, eventFunction, condition) {
        this.element.addEventListener(name, eventFunction, condition);
    }
    getElement() {
        return this.element;
    }
    setElement(element) {
        this.element = element;
    }
    getName() {
        return this.name;
    }
    setName(name) {
        this.name = name;
    }
    getLifes() {
        return this.lifes;
    }
    setLifes(lifes) {
        this.lifes = lifes;
    }
    getPosition() {
        return this.position;
    }
    setPosition(position) {
        this.position = position;
    }
    getIsOnXAxis() {
        return this.isOnXAxis;
    }
    setIsOnXAxis(isOnXAxis) {
        this.isOnXAxis = isOnXAxis;
    }
    getHasBeenSet() {
        return this.hasBeenSet;
    }
    setHasBeenSet(hasBeenSet) {
        this.hasBeenSet = hasBeenSet;
    }
    getSize() {
        return this.size;
    }
    setSize(size) {
        this.size = size;
    }
    setDockPosition(position) {
        this.element.style.top = (position * this.SIZE) + 'px';
        this.element.style.left = this.getLifes() + 'px';
    }
}
class Cell {
    constructor(j, i) {
        this.SIZE = 40;
        this.element = document.createElement('div');
        this.element.id = this.genCellID(j, i);
        this.element.className = 'cell normal';
        var topPosition = j * this.SIZE;
        var leftPosition = i * this.SIZE;
        this.element.style.top = this.genPosition(topPosition);
        this.element.style.left = this.genPosition(leftPosition);
    }
    getElement() {
        return this.element;
    }
    setElement(element) {
        this.element = element;
    }
    addEvent(name, eventFunction, condition) {
        this.element.addEventListener(name, eventFunction, condition);
    }
    genCellID(j, i) {
        return '' + j + '-' + i;
    }
    genPosition(position) {
        return position + 'px';
    }
}
class Game {
    constructor() {
        this.SIZE = 40;
        this.BOARD_INDEX = 10;
        this.GAMEBOARD = document.getElementById('gameboard');
        this.DOCKS = document.getElementById('docks');
        //game : Game;
        this.ships = new Array();
        //Event Handlers defined
        this.rotate = (event) => {
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
            }
            else {
            }
        };
        this.drag_start = (event) => {
            var style = window.getComputedStyle(event.target, null);
            event.originalEvent.dataTransfer.setData("text/plain", event.target.id);
        };
        this.drop = (event) => {
            const ship = document.getElementById(event.dataTransfer.getData("text/plain"));
            const cell = document.getElementById(event.target.id);
            this.setShip(ship, cell);
            event.preventDefault();
            return false;
        };
        this.createGame();
        //Game = this;
    }
    setMap() {
    }
    setShips(ships) {
        ships.forEach(function (element) {
            const ship = document.getElementById(element.getName());
            const cell = document.getElementById(element.getPosition().getYPosition() + '-' + element.getPosition().getXPosition());
            this.setShip(ship, cell);
        });
    }
    setShip(ship, cell) {
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
                for (var i = 1; i <= cellsRemaining; i++) {
                    let cellI = document.getElementById(origin[0] + '-' + (parseInt(origin[1]) + i));
                }
            }
            else {
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
        }
        else {
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
        for (var i = 0; i < this.BOARD_INDEX; i++) {
            for (var j = 0; j < this.BOARD_INDEX; j++) {
                // create a new div HTML element for each grid square and make it the right size
                var square = new Cell(j, i);
                if (i == 0 && j == 0) {
                    square.getElement().innerHTML = '<h1 class="left-cell-title"><span>0</span></h1><h1 class="center-cell-title" style="margin-top: -40px !important"><span>0</span></h1>';
                }
                else if (i == 0) {
                    square.getElement().innerHTML = `
        <h1 class="left-cell-title">
            <span>${j}</span></h1>`;
                }
                else if (j == 0) {
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
    }
    buildFireBoard() { }
    loadShips() {
        var carrier = new Ship(6, 'carrier');
        carrier.setDockPosition(5);
        var battleship = new Ship(5, 'battleship');
        battleship.setDockPosition(4);
        var destroyer = new Ship(4, 'destroyer');
        destroyer.setDockPosition(3);
        var submarine = new Ship(3, 'submarine');
        submarine.setDockPosition(2);
        var patrol = new Ship(2, 'patrol');
        patrol.setDockPosition(1);
        this.addToDocks(patrol);
        this.addToDocks(submarine);
        this.addToDocks(destroyer);
        this.addToDocks(battleship);
        this.addToDocks(carrier);
    }
    addToDocks(ship) {
        this.ships.push(ship);
        this.DOCKS.appendChild(ship.getElement());
        $('#' + ship.getElement().id).on('dragstart', this.drag_start);
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
    drag_enter(event) {
        event.preventDefault();
        var cell = document.getElementById(event.target.id);
        if (cell && cell.style.display != 'none')
            cell.className = 'cell hovered';
    }
    //Validation functions
    validateRotation(ship) {
        var result;
        var cell = ship.parentElement;
        var projectedHeight = this.getIntWidth(ship);
        var projectedWidth = this.getIntHeight(ship);
        var exists;
        if (projectedHeight > projectedWidth) {
            var size = (projectedHeight / this.SIZE) - 1;
            var validString = cell.id.split('-')[0];
            exists = (parseInt(validString) + size) + '-' + cell.id.split('-')[1];
        }
        else {
            var size = (projectedWidth / this.SIZE) - 1;
            var validString = cell.id.split('-')[1];
            exists = cell.id.split('-')[0] + '-' + (parseInt(validString) + size);
        }
        if (document.getElementById(exists))
            result = true;
        else
            result = false;
        return result;
    }
    evaluateDrop(ship, cell) {
        var position = cell.id.split('-');
        var size = (this.getIntWidth(ship) / this.SIZE) - 1;
        var analyzeXAxis = document.getElementById((parseInt(position[1]) + size) + '-' + position[0]);
        var analyzeYAxis = document.getElementById(position[1] + '-' + (parseInt(position[0]) + size));
        if ((ship.style.width > ship.style.height || ship.style.height == "") && analyzeXAxis) {
            return true;
        }
        else if ((ship.style.height > ship.style.width || ship.style.width == "") && analyzeYAxis) {
            return true;
        }
        return false;
    }
    //Helpers
    getIntWidth(ship) {
        if (ship.style.width == '')
            return 40;
        return parseInt(ship.style.width.replace('px', ''));
    }
    getIntHeight(ship) {
        if (ship.style.height == '')
            return 40;
        return parseInt(ship.style.height.replace('px', ''));
    }
    handleShipPositioning(ship, cell) {
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
}
document.addEventListener('DOMContentLoaded', function () {
    var game = new Game();
});
//# sourceMappingURL=Battleship.js.map