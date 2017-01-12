define('Main/hiw_intro', function () {
    return function (options) {
        var howItWorks = $(options.howItWorks);

        howItWorks.hover(
            function () {
                howItWorks.find('.red-way').animate({ opacity: 1 }, 0);
                howItWorks.find('.red-way').animate(
                    { width: '100' }, {
                        duration: 700, complete: (function () {
                            howItWorks.find('.heart').addClass('done');
                            howItWorks.find('.red-way').animate(
                                { width: '410' }, {
                                    duration: 1200, complete: (function () {
                                        howItWorks.find('.cart').addClass('done');
                                        howItWorks.find('.red-way').animate(
                                            { width: '715' }, {
                                                duration: 1000, complete: (function () {
                                                    howItWorks.find('.prize').addClass('done');
                                                    howItWorks.find('.red-way').animate(
                                                        { width: '925' }, {
                                                            duration: 500, complete: (function () {
                                                                howItWorks.find('.btn').addClass('done');
                                                            })
                                                        });

                                                })
                                            });
                                    })
                                });
                        })
                    });
            },
            function () {
                howItWorks.find('.red-way').stop().animate(
                    { width: '0', opacity: '0' }, { duration: 500 }).parent().find('.heart,.cart,.prize,.btn').removeClass('done');
            }
        );
    };
});