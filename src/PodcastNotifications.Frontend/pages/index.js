import Head from 'next/head'
import Link from 'next/link'
import Nav from '../components/nav'

export default function Home() {
  return (
    <>
      <Head>
        <title>Home | Podcast Notifications</title>
        <link rel="icon" href="/favicon.ico" />
        <link rel="stylesheet" href="https://rsms.me/inter/inter.css"></link>
      </Head>

      <Nav />

      <main className="text-center mt-40">
        <h1 className="text-4xl tracking-tight font-extrabold text-gray-900 sm:text-5xl md:text-6xl">
          Podcast Notifications
        </h1>
        <p className="mt-3 max-w-md mx-auto text-base text-gray-500 sm:text-lg md:mt-5 md:text-xl md:max-w-3xl">
          Get notified as soon as your favourite podcast episodes get released
        </p>
        <Link href="/podcasts">
          <button type="button" className="mt-5 md:mt-8 inline-flex items-center px-6 py-3 border border-transparent text-base font-medium rounded-md shadow-sm text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500" href="">
            I'm in, subscribe me to some podcasts!
          </button>
        </Link>
      </main>
    </>
  )
}
