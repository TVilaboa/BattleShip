///<reference path="typings/d3/d3.d.ts" />
///<reference path="typings/linq/linq.d.ts" />
"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Chart;
(function (Chart) {
    var Base = (function () {
        function Base(element) {
            this.element = element;
            this.iso8601 = d3.time.format('%Y-%m-%d');
            this.chartWidth = 800;
        }
        return Base;
    }());
    Chart.Base = Base;
    var Bar = (function (_super) {
        __extends(Bar, _super);
        function Bar(element) {
            _super.call(this, element);
            this.element = element;
        }
        return Bar;
    }(Base));
    Chart.Bar = Bar;
    var BubbleChart = (function (_super) {
        __extends(BubbleChart, _super);
        function BubbleChart(element) {
            _super.call(this, element);
            this.element = element;
        }
        BubbleChart.prototype.render = function (data) {
            this.bubbleChartDiameter = 300;
            var diameter = this.bubbleChartDiameter, format = d3.format(",d"), color = d3.scale.category20c();
            var bubble = d3.layout.pack()
                .sort(null)
                .size([diameter, diameter])
                .padding(1.5);
            var svg = this.element.append("svg")
                .attr("width", diameter)
                .attr("height", diameter)
                .attr("class", "bubble center-block");
            var flattened = [];
            data.forEach(function (doc) {
                flattened = flattened.concat(doc.data);
            });
            var datekey = "$.x";
            var docsByDate = Enumerable.From(flattened)
                .GroupBy(datekey, null, function (key, g) {
                return {
                    packageName: key,
                    children: { className: key, value: g.Count() }
                };
            });
            var root = new Object();
            var children = [];
            docsByDate.ForEach(function (doc) {
                var child = new Object();
                child.value = doc.children.value;
                child.depth = 0;
                child.packageName = doc.packageName;
                child.className = doc.children.className;
                children.push(child);
            });
            root.children = children;
            root.depth = 1;
            var node = svg.selectAll(".node")
                .data(bubble.nodes(root).filter(function (d) { return !d.children; }))
                .enter().append("g")
                .attr("class", "node")
                .attr("transform", function (d) { return "translate(" + d.x + "," + d.y + ")"; });
            node.append("title")
                .text(function (d) { return d.className + ": " + format(d.value); });
            node.append("circle")
                .attr("r", function (d) { return d.r; })
                .style("fill", function (d) { return color(d.packageName); });
            node.append("text")
                .attr("dy", ".3em")
                .style("text-anchor", "middle")
                .text(function (d) { return d.className; });
            this.element.style("height", diameter + "px");
        };
        // Returns a flattened hierarchy containing all leaf nodes under the root.
        BubbleChart.prototype.classes = function (root) {
            var classes = [];
            function recurse(name, node) {
                if (node.children)
                    node.children.forEach(function (child) { recurse(node.name, child); });
                else
                    classes.push({ packageName: name, className: node.name, value: node.size });
            }
            recurse(null, root);
            return { children: classes };
        };
        return BubbleChart;
    }(Base));
    Chart.BubbleChart = BubbleChart;
})(Chart || (Chart = {}));
var authorKey = "$.EnemyUserName";
var datekey = "$.Status";
var docsByAuthor = Enumerable.From(model)
    .GroupBy(authorKey, null, function (key, g) {
    return {
        author: key,
        docs: g
    };
})
    .ToArray();
var data = [];
docsByAuthor.forEach(function (element) {
    var byAuth = [];
    var desc = element.enemy;
    var docsByDate = Enumerable.From(element.docs)
        .GroupBy(datekey, null, function (key, g) {
        return {
            status: key,
            docs: g
        };
    })
        .ToArray();
    docsByDate.forEach(function (docByDate) {
        byAuth.push({ x: docByDate.status, y: docByDate.docs.Count(), z: docByDate.id });
    });
    data.push({ desc: desc, data: byAuth });
});
document.addEventListener('DOMContentLoaded', function () {
    var chart = new Chart.BubbleChart(d3.select('#bubbleChart'));
    chart.render(data);
});
//# sourceMappingURL=Data.js.map