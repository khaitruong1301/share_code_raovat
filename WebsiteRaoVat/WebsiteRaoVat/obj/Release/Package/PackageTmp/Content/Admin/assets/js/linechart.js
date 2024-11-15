new Chart(document.getElementById("linechart"), {
	type: 'line',
	data: {
		labels: ['1', '2', '3', '4', '5', '6', '7','8','9','10','11','12'],
		datasets: [{
			label: 'S? l??ng bài vi?t',
			backgroundColor: window.chartColors.navy,
			borderColor: window.chartColors.navy,
            data: [1,10,70,15,60,20,70,80],
			fill: false,
		}, {
			label: 'S? l??ng thành viên',
			fill: false,
			backgroundColor: window.chartColors.purple,
			borderColor: window.chartColors.purple,
			data: [10,40,20,35,25,50,10,70],
		}]
	},
	options: {
		responsive: true,
		// title: {
		// 	display: true,
		// 	text: 'Chart.js Line Chart'
		// },
		tooltips: {
			mode: 'index',
			intersect: false,
		},
		hover: {
			mode: 'nearest',
			intersect: true
		},
		scales: {
			xAxes: [{
				display: true,
				scaleLabel: {
					display: true,
					labelString: 'Month'
				}
			}],
			yAxes: [{
				display: true,
				scaleLabel: {
					display: true,
					labelString: 'Value'
				}
			}]
		}
	}
});
