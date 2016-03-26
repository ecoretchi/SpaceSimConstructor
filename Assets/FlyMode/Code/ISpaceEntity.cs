using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyMode{

    /// <summary>
    ///  Интерфейс для всех активных объектов в космосе, с которыми можно взаимодействовать
    /// </summary>
    /// <remarks>
    ///  Все объекты в космосе, с которыми можно взаимодействовать, должны уметь вернуть какие-то описания себя - Класс(удалено), Имя, Идентификатор (GUID)
    /// </remarks>
    internal interface ISpaceEntity {

        /// <summary>
        /// Имя объекта. Для простых объектов может просто возвращать имя GameObject'а
        /// </summary>
        string e_name { get; }

        /// <summary>
        ///  Глобальный идентификатор объекта в космосе
        /// </summary>
        /// <remarks>
        ///  GUID - идентификатор объекта, зарегистрированный в Интергалактической Регистрационной Базе (или типа того). Для незначительных объектов может быть пустой
        /// </remarks>
        string GUID { get; }
        
    }
}

