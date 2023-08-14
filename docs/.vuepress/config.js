module.exports = {
    title: 'Curly',
    themeConfig: {
        logo: '/logo_transparent.png',
        sidebar: [
            {
                title: 'Introduction',
                collapsable: true, // Whether you can collapse this section
                children: [
                    '/Introduction/WhatIsCurly.md',
                    '/Introduction/QuickStart.md'
                ]
            },
            {
                title: 'API Reference',
                collapsable: true,
                children: [
                    {
                        title: 'Core',
                        collapsable: true,
                        children: [
                            '/API-Reference/Core/CoreOverview.md'
                        ]
                    }
                ]
            }
        ]
    }
}