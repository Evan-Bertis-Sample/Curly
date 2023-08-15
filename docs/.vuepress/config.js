module.exports = {
    title: 'Curly',
    description: 'Curly Documentation',
    base: '/Curly/',
    theme: 'default-prefers-color-scheme',
    themeConfig: {
        logo: '/lil_guy_transparent.png',
        repo: 'evan-bertis-sample/curly',
        editLinkText: 'Help us improve this page!',
        docsRepo: 'evan-bertis-sample/curly',
        docsDir: 'docs',
        docsBranch: 'main',
        editLinks: true,
        overrideTheme: 'dark',
        sidebar: [
            {
                title: 'Introduction',
                collapsable: true,
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
                        collapsable: false,
                        children: [
                            '/API-Reference/Core/CoreOverview.md'
                        ]
                    }
                ]
            }
        ]
    }
}