var chart6 = c3.generate({
	bindto: '#barGraphOne',
    //data: {
    //    columns: [
    //        ['data1', 30000, 20000, 10000, 40000, 0, 0, 30000, 20000, 10000, 40000, 0, 0],
    //        ['data2', 0, 0, 0, 0, 15000, 25000, 50000, 10000, 10000, 40000, 0, 0]
    //    ],
    //    order: false,
    //    type: 'bar',
    //    groups: [
    //        ['data2', 'data3']
    //    ]
    //},

    data: {
        columns: [
            ['data1', -30, 200, 200, 400, -150, 250],
            ['data2', 130, 100, -100, 200, -150, 50],
            ['data3', -230, 200, 200, -300, 250, 250]
        ],
        type: 'bar',
        groups: [
            ['data1', 'data2']
        ]
    },
    grid: {
        y: {
            lines: [{ value: 0 }]
        }
    },
   
    axis: {
        x: {
            type: 'category',
            categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug","Sep","Oct","Nov","Dec"],
            tick: {
                format: '%b'
            }
        }
    }
});