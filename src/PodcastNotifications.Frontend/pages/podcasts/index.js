import Head from 'next/head'
import { useRouter } from 'next/router'
import Nav from '../../components/nav'
import { useState, useEffect } from 'react'

export default function Podcasts() {

    const router = useRouter();
    const [podcasts, setPodcasts] = useState([]);
    const [filteredPodcasts, setFilteredPodcasts] = useState([]);

    useEffect(() => {
        getPodcasts();
    }, []);

    async function search(input) {
        if (input.length >= 3) {
            let filtered = [];

            podcasts.forEach(index => {
                let podcastsForIndex = []
                index.data.forEach(podcast => {
                    if (podcast.title.toLowerCase().includes(input.toLowerCase() ||
                        podcast.description.toLowerCase().includes(input.toLowerCase()))) {
                            podcastsForIndex.push(podcast)
                    }
                })
                if (podcastsForIndex.length > 0) {
                    filtered.push({
                        index: index.index,
                        data: podcastsForIndex
                    })
                }
            });
            setFilteredPodcasts(filtered);
        } else {
            setFilteredPodcasts(podcasts);
        }
    }

    async function getPodcasts() {
        const result = await fetch('https://podcastnotifications.blob.core.windows.net/podcasts/index.json')
            .then(response => response.json());
        setPodcasts(result.items);
        setFilteredPodcasts(result.items);
    }

    return (
        <>
            <Head>
                <title>Podcasts | Podcast Notifications</title>
                <link rel="icon" href="/favicon.ico" />
                <link rel="stylesheet" href="https://rsms.me/inter/inter.css"></link>
            </Head>
            <div>
                <Nav />
                <div className="mt-10 max-w-full mx-auto px-4 sm:px-6 lg:px-8">
                    <div className="max-w-5xl mx-auto">

                        <div className="text-center">
                            <h1 className="text-4xl tracking-tight font-extrabold text-gray-900 sm:text-5xl md:text-6xl">
                                Podcasts
                            </h1>
                            <p className="mt-3 max-w-md mx-auto text-base text-gray-500 sm:text-lg md:mt-5 md:text-xl md:max-w-3xl">
                                Click on a podcast below to subscribe for email notifications.
                            </p>
                        </div>
                        <div className="mt-12">
                            <div className="mt-1">
                                <input
                                    type="text"
                                    name="email"
                                    id="email"
                                    className="shadow-sm focus:ring-blue-500 focus:border-blue-500 block w-full sm:text-sm border-gray-300 rounded-md"
                                    placeholder="search"
                                    onChange={(e) => search(e.target.value)}
                                />
                            </div>
                        </div>

                        <nav className="mt-10 h-full overflow-y-auto" aria-label="Podcasts">
                            {filteredPodcasts && filteredPodcasts.map((char, i) => {
                                return (
                                    <div className="relative" key={i}>
                                        <div className="z-10 sticky top-0 border-t border-b border-gray-200 bg-gray-50 px-6 py-1 text-sm font-medium text-gray-500">
                                            <h3>{char.index}</h3>
                                        </div>
                                        <ul className="relative z-0 divide-y divide-gray-200">
                                            {char.data && char.data.map((podcast, i) => {
                                                return (
                                                    <li className="bg-white cursor-pointer" onClick={() => router.push(`/podcasts/subscribe/${podcast.id}/${podcast.title}`)} key={i}>
                                                        <div className="relative px-6 py-5 flex items-center space-x-3 hover:bg-gray-50 focus-within:ring-2 focus-within:ring-inset focus-within:ring-blue-500">
                                                            <div className="flex-shrink-0">
                                                                <img className="h-10 w-10 rounded-full" src={podcast.imageUrl} alt="" />
                                                            </div>
                                                            <div className="flex-1 min-w-0">
                                                                <div className="focus:outline-none">
                                                                    <span className="absolute inset-0" aria-hidden="true"></span>
                                                                    <p className="text-sm font-medium text-gray-900">
                                                                        {podcast.title}
                                                                    </p>
                                                                    <p className="text-sm text-gray-500 truncate">
                                                                        {podcast.description}
                                                                    </p>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </li>
                                                )
                                            })}
                                        </ul>
                                    </div>
                                )
                            })}
                        </nav>
                    </div>
                </div>
            </div>
        </>
    )
}