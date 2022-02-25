namespace Core.Timers
{
    public interface ITimer
    {
        /// <summary>
        /// Перезапускает таймер.
        /// </summary>
        /// <param name="seconds"></param>
        public void Run(float seconds);

        /// <summary>
        /// Запускает или продолжает таймер.
        /// </summary>
        public void Run();

        /// <summary>
        /// Ставит таймер на паузу.
        /// </summary>
        public void Pause();

        /// <summary>
        /// Сбрасывает таймер.
        /// </summary>
        public void Stop();
    }
}