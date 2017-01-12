define(
    'Buy/widgets', [
        'Buy/Widgets/zipcode',
        'Buy/Widgets/history-locations',
        'Buy/Widgets/delivery-variants',
        'Buy/Widgets/proceed-button',
        'Buy/Widgets/address',
        'Buy/Widgets/delivery-item',
        'Buy/Widgets/advance'],
    function(zipcode, historyLocations, deliveryVariants, proceedButton, address, deliveryItem, advance) {
        return {
            zipcode: zipcode,
            historyLocations: historyLocations,
            deliveryVariants: deliveryVariants,
            proceedButton: proceedButton,
            address: address,
            deliveryItem: deliveryItem,
            advance: advance
        };
    });
