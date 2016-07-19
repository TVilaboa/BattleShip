///<reference path="typings/d3/d3.d.ts" />
///<reference path="typings/linq/linq.d.ts" />
"use strict";
var Chart;
(function (Chart) {
    class Base {
        constructor(element) {
            this.element = element;
            this.iso8601 = d3.time.format('%Y-%m-%d');
            this.chartWidth = 800;
        }
    }
    Chart.Base = Base;
    class Bar extends Base {
        constructor(element) {
            super(element);
            this.element = element;
        }
    }
    Chart.Bar = Bar;
    class BubbleChart extends Base {
        constructor(element) {
            super(element);
            this.element = element;
        }
        render(data) {
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
            data.forEach(doc => {
                flattened = flattened.concat(doc.data);
            });
            let datekey = "$.x";
            let docsByDate = Enumerable.From(flattened)
                .GroupBy(datekey, null, function (key, g) {
                return {
                    packageName: key,
                    children: { className: key, value: g.Count() }
                };
            });
            let root = new Object();
            let children = [];
            docsByDate.ForEach(doc => {
                let child = new Object();
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
        }
        // Returns a flattened hierarchy containing all leaf nodes under the root.
        classes(root) {
            var classes = [];
            function recurse(name, node) {
                if (node.children)
                    node.children.forEach(function (child) { recurse(node.name, child); });
                else
                    classes.push({ packageName: name, className: node.name, value: node.size });
            }
            recurse(null, root);
            return { children: classes };
        }
    }
    Chart.BubbleChart = BubbleChart;
})(Chart || (Chart = {}));
let authorKey = "$.EnemyUserName";
let datekey = "$.Status";
let docsByAuthor = Enumerable.From(model)
    .GroupBy(authorKey, null, function (key, g) {
    return {
        author: key,
        docs: g
    };
})
    .ToArray();
var data = [];
docsByAuthor.forEach(element => {
    var byAuth = [];
    let desc = element.enemy;
    let docsByDate = Enumerable.From(element.docs)
        .GroupBy(datekey, null, function (key, g) {
        return {
            status: key,
            docs: g
        };
    })
        .ToArray();
    docsByDate.forEach(docByDate => {
        byAuth.push({ x: docByDate.status, y: docByDate.docs.Count(), z: docByDate.id });
    });
    data.push({ desc: desc, data: byAuth });
});
document.addEventListener('DOMContentLoaded', function () {
    var chart = new Chart.BubbleChart(d3.select('#bubbleChart'));
    chart.render(data);
});
//# sourceMappingURL=Data.js.map