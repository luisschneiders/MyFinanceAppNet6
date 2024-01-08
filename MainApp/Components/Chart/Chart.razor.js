export function setupChart(chartId, config, position) {
    const options = {
        plugins: {
            legend: {
                position: position
            }
        },
        indexAxis: config.option.indexAxis,
    };

    const ctx = document.getElementById(chartId).getContext('2d');
    config.options = options;

    const chart = new Chart(ctx, config);

    return chart;
}

export function updateChartData(chart, data) {
    chart.data = data;
    chart.update();
}

export function removeChartData(chart) {
    chart.data.datasets.forEach((dataset) => {
        dataset.data = [];
    });

    chart.update();
}
