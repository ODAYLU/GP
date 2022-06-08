//chart 1

var ctx = document.getElementById('myChart').getContext('2d');
var myChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: ['غزة', 'رفح', 'خانيونس', 'بيت حانون', 'النصيرات', 'S', 'S'],
        datasets: [{
            label: 'عدد العقارت',
            data: [12, 19, 3, 17, 6, 3, 7],
            backgroundColor: "#db7d7dd6"
        }, {
            label: 'عدد المسجلين',
            data: [2, 29, 5, 5, 2, 3, 10],
            backgroundColor: "#39BF99d6"
        }]
    }
});


//chart 2



var xValues = ["غزة", "رفح", "خانيونس", "معسكر جباليا"];
var yValues = [55, 49, 44, 15];
var barColors = [
    "#b91d47",
    "#00aba9",
    "#2b5797",
    "#e8c3b9",
];

new Chart("myChart2", {
    type: "doughnut",
    data: {
        labels: xValues,
        datasets: [{
            backgroundColor: barColors,
            data: yValues
        }]
    },
    options: {
        title: {
            display: true,
        }
    }
});