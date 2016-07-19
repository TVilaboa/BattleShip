/**
 * Describes Board Position for Elements
 */
var BoardPosition = (function () {
    function BoardPosition() {
    }
    BoardPosition.prototype.setXPosition = function (xValue) {
        this.xPosition = xValue;
    };
    BoardPosition.prototype.getXPosition = function () {
        return this.xPosition;
    };
    BoardPosition.prototype.setYPosition = function (yValue) {
        this.yPosition = yValue;
    };
    BoardPosition.prototype.getYPosition = function () {
        return this.yPosition;
    };
    BoardPosition.prototype.setCoordinates = function (xValue, yValue) {
        this.xPosition = xValue;
        this.yPosition = yValue;
    };
    return BoardPosition;
}());
var Ship = (function () {
    function Ship(lifes, name) {
        this.SIZE = 40;
        this.element = document.createElement('div');
        this.element.id = name;
        this.element.className = 'ships';
        this.element.style.width = (lifes * this.SIZE) + 'px';
        var image = '/Content/Images/ship.png';
        this.element.style.backgroundImage = "url('" + image + "')";
        this.element.style.backgroundSize = (lifes * this.SIZE) + "px" + " 40px ";
        this.element.draggable = true;
        this.element.style.zIndex = '1';
        this.lifes = lifes;
        this.name = name;
        this.size = lifes;
    }
    Ship.prototype.addEvent = function (name, eventFunction, condition) {
        this.element.addEventListener(name, eventFunction, condition);
    };
    Ship.prototype.getElement = function () {
        return this.element;
    };
    Ship.prototype.setElement = function (element) {
        this.element = element;
    };
    Ship.prototype.getName = function () {
        return this.name;
    };
    Ship.prototype.setName = function (name) {
        this.name = name;
    };
    Ship.prototype.getLifes = function () {
        return this.lifes;
    };
    Ship.prototype.setLifes = function (lifes) {
        this.lifes = lifes;
    };
    Ship.prototype.getPosition = function () {
        return this.position;
    };
    Ship.prototype.setPosition = function (position) {
        this.position = position;
    };
    Ship.prototype.getIsOnXAxis = function () {
        return this.isOnXAxis;
    };
    Ship.prototype.setIsOnXAxis = function (isOnXAxis) {
        this.isOnXAxis = isOnXAxis;
    };
    Ship.prototype.getHasBeenSet = function () {
        return this.hasBeenSet;
    };
    Ship.prototype.setHasBeenSet = function (hasBeenSet) {
        this.hasBeenSet = hasBeenSet;
    };
    Ship.prototype.getSize = function () {
        return this.size;
    };
    Ship.prototype.setSize = function (size) {
        this.size = size;
    };
    Ship.prototype.setDockPosition = function (position) {
        this.element.style.top = (position * this.SIZE) + 'px';
        this.element.style.left = this.getLifes() + 'px';
    };
    return Ship;
}());
var Cell = (function () {
    function Cell(j, i) {
        this.SIZE = 40;
        this.element = document.createElement('div');
        this.element.id = this.genCellID(j, i);
        this.element.className = 'cell normal';
        var topPosition = j * this.SIZE;
        var leftPosition = i * this.SIZE;
        this.element.style.top = this.genPosition(topPosition);
        this.element.style.left = this.genPosition(leftPosition);
    }
    Cell.prototype.getElement = function () {
        return this.element;
    };
    Cell.prototype.setElement = function (element) {
        this.element = element;
    };
    Cell.prototype.addEvent = function (name, eventFunction, condition) {
        this.element.addEventListener(name, eventFunction, condition);
    };
    Cell.prototype.genCellID = function (j, i) {
        return '' + j + '-' + i;
    };
    Cell.prototype.genPosition = function (position) {
        return position + 'px';
    };
    return Cell;
}());
var Game = (function () {
    function Game() {
        this.SIZE = 40;
        this.BOARD_INDEX = 10;
        this.GAMEBOARD = document.getElementById('gameboard');
        this.DOCKS = document.getElementById('docks');
        this.ships = new Array();
        this.createGame();
        this.game = this;
    }
    Game.prototype.setMap = function () {
    };
    Game.prototype.setShips = function (ships) {
        ships.forEach(function (element) {
            var ship = document.getElementById(element.getName());
            var cell = document.getElementById(element.getPosition().getYPosition() + '-' + element.getPosition().getXPosition());
            this.setShip(ship, cell);
        });
    };
    Game.prototype.setShip = function (ship, cell) {
        cell.className = 'cell normal';
        if (this.evaluateDrop(ship, cell)) {
            var shipOnArray = this.ships.filter(function (s) { return s.getName() === ship.id; })[0];
            shipOnArray.getPosition().setXPosition(parseInt(cell.id.split('-')[1]));
            shipOnArray.getPosition().setYPosition(parseInt(cell.id.split('-')[0]));
            //ship.style.background = 'orange';
            var image = '@Url.Content("~/Content/Images/ship.png")';
            ship.style.backgroundImage = "url('" + image + "')";
            var shipHeight = this.getIntHeight(ship);
            var shipWidth = this.getIntWidth(ship);
            ship.style.backgroundSize = shipWidth + "px " + shipHeight + "px";
            if (ship.style.width > ship.style.height) {
                var cellsRemaining = (shipWidth / this.SIZE) - 1;
                var origin = cell.id.split('-');
                for (var i = 1; i <= cellsRemaining; i++) {
                    var cellI = document.getElementById(origin[0] + '-' + (parseInt(origin[1]) + i));
                }
            }
            else {
                var cellsRemaining = (shipHeight / this.SIZE) - 1;
                var origin = cell.id.split('-');
                for (i = 1; i <= cellsRemaining; i++) {
                    var cellI = document.getElementById((parseInt(origin[0]) + i) + '-' + origin[1]);
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
    };
    Game.prototype.createGame = function () {
        $("#waiter").css("visibility", "hidden");
        $("#actions").css("visibility", "visible");
        this.buildBoard();
        this.loadShips();
        var text = "Set your ships!!";
        //this.showNotification(text);
    };
    Game.prototype.buildBoard = function () {
        for (var i = 0; i < this.BOARD_INDEX; i++) {
            for (var j = 0; j < this.BOARD_INDEX; j++) {
                // create a new div HTML element for each grid square and make it the right size
                var square = new Cell(j, i);
                //@AddCellTiltle();
                this.GAMEBOARD.appendChild(square.getElement());
                square.addEvent('dragenter', this.drag_enter, false);
                square.addEvent('dragleave', this.drag_leave, false);
                square.addEvent('dragenter', this.drag_over, false);
                square.addEvent('drop', this.drop, false);
                document.body.addEventListener('dragover', this.drag_over, false);
            }
        }
    };
    Game.prototype.startGame = function () {
    };
    Game.prototype.buildFireBoard = function () { };
    Game.prototype.loadShips = function () {
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
    };
    Game.prototype.addToDocks = function (ship) {
        this.ships.push(ship);
        $('#' + ship.getElement()).on('dragstart', (this.drag_start));
        this.DOCKS.appendChild(ship.getElement());
    };
    //Event Handlers defined
    Game.prototype.rotate = function (event) {
        event.preventDefault();
        var ship = document.getElementById(event.target.id);
        if (this.validateRotation(ship)) {
            var shipInArray = this.ships.filter(function (s) { return s.getName() === event.target.id; })[0];
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
    Game.prototype.drag_start = function (event) {
        var style = window.getComputedStyle(event.target, null);
        event.originalEvent.dataTransfer.setData("text/plain", event.target.id);
    };
    Game.prototype.drag_over = function (event) {
        event.preventDefault();
        return false;
    };
    Game.prototype.drag_leave = function (event) {
        event.preventDefault();
        var cell = document.getElementById(event.target.id);
        if (cell && cell.style.display != 'none')
            cell.className = 'cell normal';
    };
    Game.prototype.drag_enter = function (event) {
        event.preventDefault();
        var cell = document.getElementById(event.target.id);
        if (cell && cell.style.display != 'none')
            cell.className = 'cell hovered';
    };
    Game.prototype.drop = function (event) {
        var ship = document.getElementById(event.dataTransfer.getData("text/plain"));
        var cell = document.getElementById(event.target.id);
        this.setShip(ship, cell);
        event.preventDefault();
        return false;
    };
    //Validation functions
    Game.prototype.validateRotation = function (ship) {
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
    };
    Game.prototype.evaluateDrop = function (ship, cell) {
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
    };
    //Helpers
    Game.prototype.getIntWidth = function (ship) {
        if (ship.style.width == '')
            return 40;
        return parseInt(ship.style.width.replace('px', ''));
    };
    Game.prototype.getIntHeight = function (ship) {
        if (ship.style.height == '')
            return 40;
        return parseInt(ship.style.height.replace('px', ''));
    };
    Game.prototype.handleShipPositioning = function (ship, cell) {
        var size = (this.getIntWidth(ship) / this.SIZE) - 1;
        var position = cell.id.split('-');
        var exists = position[0] + '-' + (parseInt(position[1]) + size);
        if (!document.getElementById(exists)) {
            var width = ship.style.width;
            var height = ship.style.height;
            ship.style.height = width;
            ship.style.width = height;
            var shipInArray = this.ships.filter(function (s) { return s.getName() === ship.id; })[0];
            shipInArray.setIsOnXAxis(false);
        }
    };
    return Game;
}());
document.addEventListener('DOMContentLoaded', function () {
    var game = new Game();
});
//# sourceMappingURL=Battleship.js.map