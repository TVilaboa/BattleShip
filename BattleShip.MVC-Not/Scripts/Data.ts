///<reference path="typings/d3/d3.d.ts" />
///<reference path="typings/linq/linq.d.ts" />
"use strict";




interface IByAuthor {
    desc: string;
    data: IByAuthorByDate[];
}

interface IByAuthorByDate {
    x: Date;
    y: number;
}

interface IDoc
{
    author: String;
    published: Date;
    content: String[];
}

interface ID3DateNode extends ID3Node {
    className?: String;
    packageName?: String;
}



module Chart {

   

    export class Base {
        public iso8601 = d3.time.format('%Y-%m-%d');
        public chartWidth = 800;

        constructor(public element) { }
    }

    export class Bar extends Base {
        public element: ID3Selection;
        constructor(element: ID3Selection) {
            super(element);
            this.element = element;
        }

        public chartHeight = 1000;
        public chartWidth = 1400;
        public legendItemHeight = 30;
        public legendWidth = 150;
        //public colors = ['rgb(0, 113, 188)', 'rgb(0, 174, 239)', 'rgb(145, 0, 145)', 'rgb(145, 0, 15)', 'rgb(15, 23, 145)'];
        public xAxisHashHeight = 5;
        public layout = 'wiggle';


        public render(data: IByAuthor[]) {

            let color = d3.scale.category20c();
            // Create stack layout
            var stackLayout = d3.layout.stack()
                .values((d: IByAuthor) => d.data)
                .offset(this.layout);

            var stackData = stackLayout(data);


            var flattened: IByAuthorByDate[] = [];
            data.forEach(doc => {
                flattened = flattened.concat(doc.data);
            });
            let datekey: any = "$.x.getTime()";
            let docsByDate = Enumerable.From(flattened).Where("x => x.y > 0")
                .GroupBy(datekey, null,
                    function (key, g) {
                        return {
                            date: key,
                            docs: g
                        }
                    }); // Maximum measurement in the dataset
            //var maxY = d3.max(stackData, (d) => d3.max<any, any>(d.data, (d) => d.y0 + d.y));
            var maxY = docsByDate.Max("$.docs.Sum('$.y')");
            //var maxSummedY = d3.max<IByAuthor,number>(data, (d):number => Enumerable.From(d.data).Sum("$.y"));
            // Earliest day in the dataset
            var minX: Date = d3.time.day.offset(d3.min(data, (d) => d3.min(d.data, (d) => d.x)),-1);
            var maxX: Date = d3.time.day.offset(d3.max(data, (d) => d3.max(d.data, (d) => d.x)),1);

            // All days in the dataset (from earliest day until now)
            var days = d3.time.days(minX, maxX);

            // Area of the region containing the bars
            var areaWidth = this.chartWidth - this.legendWidth;

            var barWidth = areaWidth / days.length;

            // Create scales for X and Y axis (X based on dates, Y based on performance data)
            var x = d3.time.scale()
                .domain([minX, maxX])
                .range([0, this.chartWidth - this.legendWidth]);
            var y = d3.scale.linear()
                .domain([0, maxY])
                .range([0, this.chartHeight]);
            var ticks = x.ticks(d3.time.mondays, 1);

            // SVG element
            var svg = this.element.append('svg')
                .attr('height', this.chartHeight + 25)
                .attr('width', this.chartWidth);

            // Groups that contain bar segments for each dataset
            var barGroups = svg.selectAll('g.bars')
                .data(stackData)
                .enter().append('g')
                .attr('class', 'bars')
                //.style('fill', (d, i) => this.colors[<number>i])
                .style("fill", function (d) { return color(d.desc); })
                .attr('transform', 'translate(' + this.legendWidth + ', 0)');
                
            // Legend
            var legendGroup = svg.append('g')
                .attr('class', 'legend');

            // Legend items
            var legendItem = legendGroup.selectAll('g.legendItem')
                .data(stackData)
                .enter().append('g')
                .attr('class', 'legendItem')
                //.style('fill', (d, i) => this.colors[<number>i])
                .style("fill", function (d) { return color(d.desc); })
                .attr('transform', (d, i) => 'translate(0, ' + (this.legendItemHeight * (25 - i)) + ')')
                .attr('data-author',(d) => d.desc);
               

            legendItem.append('rect')
                .attr('width', 25)
                .attr('height', 25);
                

            legendItem.append('text')
                .text((d) => d.desc)
                .attr('x', 30)
                .attr('dy', '1em');
               


            // Bars
            var rects = barGroups.selectAll('rect')
                .data((d) => d.data)
                .enter()
                .append('rect')
                .attr('x', function(d, i){ return x(d.x); })
                .attr('y', (d, i) => {
                    if (d.y === 0) return 0;
                    
                     return this.chartHeight - y(d.y + d.y0);
                })
                .attr('width', barWidth)
                .attr('height', (d, i) => y(d.y));

            // Add title (mouseover popup) to bars           
            rects.append('title')
                .text((d) => this.iso8601(d.x) + ' - ' + d.y + 'articles');
                


            // Add an axis marker to the bottom
            var axis = d3.svg.axis();
            axis.scale(x)
                .ticks(d3.time.days, 1)
                .tickSubdivide(6)
                .tickFormat(this.iso8601)
                .tickSize(10, 5, 0);

            var axisGroup = svg.append('g')
                .attr('class', 'axis')
                .attr('transform', 'translate(' + this.legendWidth + ',' + this.chartHeight + ')')
                .call(axis);

        }
    }

    export class BubbleChart extends Base {
        public element: ID3Selection;
        constructor(element: ID3Selection) {
            super(element);
            this.element = element;
        }

        public render(data: IByAuthor[]) {
            var diameter = 960,
                format = d3.format(",d"),
                color = d3.scale.category20c();

            var bubble = d3.layout.pack()
                .sort(null)
                .size([diameter, diameter])
                .padding(1.5);

            var svg = this.element.append("svg")
                .attr("width", diameter)
                .attr("height", diameter)
                .attr("class", "bubble");

            var flattened: IByAuthorByDate[] = [];
            data.forEach(doc => {
                flattened = flattened.concat(doc.data);
            });
            let datekey: any = "$.x.getTime()";
            let docsByDate = Enumerable.From(flattened).Where("x => x.y > 0")
                .GroupBy(datekey, null,
                function (key, g) {
                    return {
                        packageName: new Date(key).toDateString(),
                        children: { className: new Date(key).toDateString(), value: g.Count()}
                    }
                });
            let root: ID3DateNode = new Object();
            let children: ID3DateNode[] = [];
            docsByDate.ForEach(doc => {
                let child: ID3DateNode = new Object();
                child.value = doc.children.value;
                child.depth = 0;
                child.packageName = doc.packageName;
                child.className = doc.children.className;
                children.push(child);
            });
            root.children = children;
            root.depth = 1;
           
                var node = svg.selectAll(".node")
                    //.data(bubble.nodes(this.classes(docsByDate))
                    //    .filter(function (d) { return !d.children; }))
                    //.data(docsByDate.ToArray())
                    .data(bubble.nodes(root).filter(function (d) { return !d.children; }))
                        
                    .enter().append("g")
                    .attr("class", "node")
                    .attr("transform", function (d) { return "translate(" + d.x + "," + d.y + ")"; });

                node.append("title")
                    .text(function (d) { return d.className  + ": " + format(d.value); });

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
    private classes(root) {
        var classes = [];

        function recurse(name, node) {
            if (node.children) node.children.forEach(function (child) { recurse(node.name, child); });
            else classes.push({ packageName: name, className: node.name, value: node.size });
        }

        recurse(null, root);
        return { children: classes };
    }

   

    }
}




declare var model: IDoc[];
declare var indexWithAuthor: string;
let authorKey: any = "$.author";
let datekey: any = "$.published"; 
let docsByAuthor = Enumerable.From(model)
    .GroupBy(authorKey, null,
    function (key, g) {
        return {
            author: key,
            docs: g
        }
    })
    .ToArray();
var data = [];
let maxDocLenght : number = docsByAuthor[0].docs.Count();
let averageDocLenght : number = docsByAuthor[0].docs.Count();
let mustNormalize: boolean = false;
docsByAuthor.forEach(element => {
    var byAuth = [];
    element.docs.ForEach(doc => {
        doc.published = new Date(new Date(doc.published).getFullYear(), new Date(doc.published).getMonth(), new Date(doc.published).getDate()).toString();
    });
    let desc :string = element.author;
   let docsByDate = Enumerable.From(element.docs)
        .GroupBy(datekey, null,
        function (key, g) {
            return {
                date: key,
                docs: g
            }
        })
        .ToArray();
    docsByDate.forEach(docByDate => {
        byAuth.push({ x: new Date(docByDate.date), y: docByDate.docs.Count(),z: docByDate.id });
   });
    if (averageDocLenght !== byAuth.length) {
        if (byAuth.length > maxDocLenght) {
            maxDocLenght = byAuth.length;
        }
        mustNormalize = true;
    }
    data.push(<any>{ desc: desc, data: byAuth });

});
let dates: Date[] = [];
Enumerable.From(model).Distinct("$.published").ForEach(doc => {
    dates.push(new Date(new Date(doc.published).getFullYear(), new Date(doc.published).getMonth(), new Date(doc.published).getDate()));
});

if (mustNormalize) {
    data.forEach(dataElement => {
        if (dataElement.data.length < maxDocLenght) {
            dates.forEach(date => {
                if (!Enumerable.From(dataElement.data).Contains("x => x.x.getTime() == " + date.getTime())) {
                    dataElement.data.push({ x: date, y: 0 });
                }
            });
            //for (var i = dataElement.data.length; i < maxDocLenght; i++) {
            //    dataElement.data.push({ x: data[0].data[0].x, y: 0});
            //}
            dataElement.data.sort(function (a: IByAuthorByDate, b: IByAuthorByDate) {
                return a.x.getTime() - b.x.getTime();
            });
        }
    });
}
//data.sort(function(a: IByAuthor, b: IByAuthor) {
//    return a.desc.localeCompare(b.desc);
//});

document.addEventListener('DOMContentLoaded', function () {
    var chart = new Chart.BubbleChart(d3.select('#bubbleChart'));

    chart.render(data);

  
    var perfchart = new Chart.Bar(d3.select('#customChart'));
    perfchart.render(data);
    $('g').click(function(event) {
        if ($(this).data('author')) {
            window.location.href = encodeURI(indexWithAuthor.replace('xxx', $(this).data('author')));
            
            } 
        
       
    });
});