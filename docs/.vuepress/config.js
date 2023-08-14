module.exports = {
    title: 'Curly',
    description: 'Curly is a lightweight unity template that promotes solid architecture, and removes the redundancy of implementing systems found in almost all games.',
    base: '/Curly/',
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